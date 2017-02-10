﻿using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace Slate{

	[System.Serializable]
	///Defines a parameter (property/field) that can be animated with curves
	public class AnimatedParameter : IAnimatableData{

		///Used for storing the meta data
		[System.Serializable]
		class SerializationMetaData{
			public bool enabled = true;
			public string parameterName;
			public string declaringTypeName;
			public string transformHierarchyPath;
			public ParameterType parameterType;

			public Type declaringType{get;private set;}
			public PropertyInfo property{get;private set;}
			public FieldInfo field{get;private set;}
			public Type animatedType{get;private set;}
			public void Deserialize(){
				declaringType = RuntimeTools.GetType(declaringTypeName);
				if (declaringType != null){
					property      = declaringType.RTGetProperty(parameterName);
					field         = declaringType.RTGetField(parameterName);
					animatedType  = property != null? property.PropertyType : field != null? field.FieldType : null;
				}
			}
		}

		///The type of the parameter.
		///This is an enum in case more are added.
		public enum ParameterType{
			NotSet,
			Property,
			Field
		}

		private const float COMPARE_THRESHOLD = 0.01f;
		private const float PROXIMITY_TOLERANCE = 0.05f;

		[SerializeField]
		private string _serializedData;
		[SerializeField]
		private AnimationCurve[] _curves;

		///Is the parameter enabled and will be used?
		public bool enabled{
			get {return data.enabled;}
		}

		///Set the parameter enabled/disabled
		public void SetEnabled(bool value, object obj, float time){
			if (enabled != value){
				data.enabled = value;
				serializedData = JsonUtility.ToJson(data);
				if (value == false){
					if (snapshot != null){ //revert before resetting
						SetCurrentValue(obj, snapshot);
					}
				}
				NotifyChange();
			}
		}

		//Serialized json data
		private string serializedData{
			get {return _serializedData;}
			set {_serializedData = value;}
		}

		//Serialization structure
		[System.NonSerialized]
		private SerializationMetaData _data;
		private SerializationMetaData data{
			get
			{
				if (_data != null){
					return _data;
				}

				_data = JsonUtility.FromJson<SerializationMetaData>(serializedData);
				_data.Deserialize();
				return _data;
			}
		}

		///The animation curves of the parameter
		public AnimationCurve[] curves{
			get {return _curves;}
			private set {_curves = value;}
		}

		///Get the info from the deserialized data. These are basicaly shortcuts
		public string parameterName {get {return data.parameterName;}}
		public Type animatedType{ get {return data.animatedType;} }
		public ParameterType parameterType{ get {return data.parameterType;} }
		public string transformHierarchyPath{ get {return data.transformHierarchyPath;} }
		public Type declaringType{	get	{return data.declaringType;} }
		public PropertyInfo property{ get {return data.property; } }
		public FieldInfo field{ get {return data.field; } }
		public bool isProperty{ get {return parameterType == ParameterType.Property;} }
		///

		//The snapshot value before evaluation
		private object snapshot {get; set;}
		//The last evaluated value. Mostly used to check value changes
		private object currentEval {get; set;}
		//Used to virtualy parent transform based parameters if not null
		private Transform context {get; set;}
		//The IKeyable reference this parameter belongs to
		private IKeyable keyable {get; set;}

		///The types supported for animation
		public static readonly Type[] supportedTypes = new Type[]{
			typeof(bool),
			typeof(int),
			typeof(float),
			typeof(Vector2),
			typeof(Vector3),
			typeof(Color)
		};

		[System.NonSerialized]
		private object _animatableAttribute; ///Cache of possible AnimatableParameter attribute usage for declaring this parameter
		public AnimatableParameterAttribute animatableAttribute{
			get
			{
				if (_animatableAttribute == null){
					var m = GetMemberInfo();
					if (m == null){	return null; }
					var att = m.RTGetAttribute<AnimatableParameterAttribute>(true);
					_animatableAttribute = att != null? att : new object();
				}
				return _animatableAttribute as AnimatableParameterAttribute;
			}
		}

		///External means that the parameter was not created for internal use by AnimatableParameter attribute.
		public bool isExternal{
			get	{ return animatableAttribute == null; }
		}
	
		///shortcuts for curves
		private AnimationCurve curve1{
			get {return curves[0];}
			set {curves[0] = value;}
		}
		private AnimationCurve curve2{
			get {return curves[1];}
			set {curves[1] = value;}
		}
		private AnimationCurve curve3{
			get {return curves[2];}
			set {curves[2] = value;}
		}
		private AnimationCurve curve4{
			get {return curves[3];}
			set {curves[3] = value;}
		}
		////

		///Is all good to go?
		public bool isValid{
			get
			{
				if (string.IsNullOrEmpty(serializedData) || data == null){
					return false;
				}
				return isProperty? property != null : field != null;
			}
		}

		///The PropertyInfo or FieldInfo used
		public MemberInfo GetMemberInfo(){
			if (isValid){
				return isProperty? (MemberInfo)property : (MemberInfo)field;
			}
			return null;
		}


		///The curves used
		public AnimationCurve[] GetCurves(){
			return curves;
		}

		///Creates a new animated parameter out of a member info that exists on target object and optionaly is a component type under the root transform.
		public AnimatedParameter(MemberInfo member, object obj, Transform root){
			if (member is PropertyInfo){
				InitWithProperty( (PropertyInfo)member, obj, root);
				return;
			}
			if (member is FieldInfo){
				InitWithField( (FieldInfo)member, obj, root);
				return;
			}
			Debug.LogError("MemberInfo provided is neither Property nor Field");
		}

		//initialize with PropertyInfo
		void InitWithProperty(PropertyInfo targetProperty, object obj, Transform root){

			if ( !supportedTypes.Contains(targetProperty.PropertyType) ){
				Debug.LogError(string.Format("Type '{0}' is not supported for animation", targetProperty.PropertyType) );
				return;				
			}

			if (!targetProperty.CanRead || !targetProperty.CanWrite){
				Debug.LogError("Animated Property must be able to both read and write");
				return;
			}

			if (targetProperty.RTGetGetMethod().IsStatic){
				Debug.LogError("Static Properties are not supported");
				return;
			}

			var newData               = new SerializationMetaData();
			newData.parameterType     = ParameterType.Property;
			newData.parameterName     = targetProperty.Name;
			newData.declaringTypeName = targetProperty.RTReflectedType().FullName;

			if (obj is Component && root != null){
				newData.transformHierarchyPath = CalculateTransformPath( root, (obj as Component).transform );
			}

			serializedData = JsonUtility.ToJson(newData);

			InitializeCurves();
		}

		//initialize with FieldInfo
		void InitWithField(FieldInfo targetField, object obj, Transform root){

			if ( !supportedTypes.Contains(targetField.FieldType) ){
				Debug.LogError(string.Format("Type '{0}' is not supported for animation", targetField.FieldType) );
				return;
			}

			if (targetField.IsStatic){
				Debug.LogError("Static Fields are not supported");
				return;
			}

			var newData               = new SerializationMetaData();
			newData.parameterType     = ParameterType.Field;
			newData.parameterName     = targetField.Name;
			newData.declaringTypeName = targetField.RTReflectedType().FullName;

			if (obj is Component && root != null){
				newData.transformHierarchyPath = CalculateTransformPath( root, (obj as Component).transform );
			}

			serializedData = JsonUtility.ToJson(newData);

			InitializeCurves();
		}


		///returns true if this animated parameter already points to the provided one
		public bool CompareTo(AnimatedParameter animParam){
			if (parameterName == animParam.parameterName &&
				declaringType == animParam.declaringType &&
				transformHierarchyPath == animParam.transformHierarchyPath)
			{
				return true;
			}
			return false;
		}


		string CalculateTransformPath(Transform root, Transform target){
			var path = new List<string>();
			var curr = target;
			while(curr != root && curr != null){
				path.Add(curr.name);
				curr = curr.parent;
			}
			path.Reverse();
			return string.Join("/", path.ToArray());
		}

		Transform ResolveTransformPath(Transform root){
			var transform = root;
			foreach(var tName in transformHierarchyPath.Split('/')){
				transform = transform.Find(tName);
				if (transform == null){
					return null;
				}
			}
			return transform;
		}

		void InitializeCurves(){

			if (animatedType == typeof(bool)){
				curves = new AnimationCurve[1];
				curve1 = new AnimationCurve();
				return;
			}
			if (animatedType == typeof(int)){
				curves = new AnimationCurve[1];
				curve1 = new AnimationCurve();
				return;
			}
			if (animatedType == typeof(float)){
				curves = new AnimationCurve[1];
				curve1 = new AnimationCurve();
				return;
			}
			if (animatedType == typeof(Vector2)){
				curves = new AnimationCurve[2];
				curve1 = new AnimationCurve();
				curve2 = new AnimationCurve();
				return;
			}
			if (animatedType == typeof(Vector3)){
				curves = new AnimationCurve[3];
				curve1 = new AnimationCurve();
				curve2 = new AnimationCurve();
				curve3 = new AnimationCurve();
				return;
			}
			if (animatedType == typeof(Color)){
				curves = new AnimationCurve[4];
				curve1 = new AnimationCurve();
				curve2 = new AnimationCurve();
				curve3 = new AnimationCurve();
				curve4 = new AnimationCurve();
				return;
			}

			Debug.LogError(string.Format("Parameter Type '{0}' is not supported", animatedType.Name));
		}

		///Set the virtual parent for transform based parameters
		public void SetTransformContext(Transform context){
			this.context = context;
		}

		///Store a snapshot for restoring later.
		public void SetSnapshot(object obj){

			if (!isValid){
				return;
			}

			//always save snapshot even if disabled for in case it get's enabled
			snapshot = GetCurrentValue(obj);

			#if UNITY_EDITOR
			if ( (!Application.isPlaying && enabled && Prefs.autoFirstKey && !HasAnyKey() ) ){
				SetKeyCurrent(obj, 0);
			}
			#endif
		}

		///Restore the snapshot on the target
		public void RestoreSnapshot(object obj){

			if (!isValid || !enabled){
				return;
			}

			if (snapshot != null){
				if (isExternal){
					SetCurrentValue(obj, snapshot);
				}
				currentEval = null;
				snapshot = null;
			}
		}


		///Resolves the final object to be used.
		[System.NonSerialized]
		private object _resolved = null;
		public object ResolvedObject(object obj){

			if (obj == null || obj.Equals(null)){
				return null;
			}

			//if snapshot not null, means at least one time this has been evaluated
			if (snapshot != null && _resolved != null && !_resolved.Equals(null) ){
				return _resolved;
			}

			if (obj is GameObject){

				if ( !string.IsNullOrEmpty(transformHierarchyPath) ){
					var transform = ResolveTransformPath( (obj as GameObject).transform );
					return _resolved = transform != null? transform.GetComponent(declaringType) : null;
				}

				return _resolved = (obj as GameObject).GetComponent(declaringType);
			}

			return _resolved = obj;
		}


		//Gets the current raw property value from target
		public object GetCurrentValue(object obj){

			if (!isValid){
				return null;
			}
			
			obj = ResolvedObject(obj);
			if (obj == null || obj.Equals(null)){
				return null;
			}

			if (obj is Transform){ //special treat
				var transform = obj as Transform;
				var value = default(Vector3);
				if (parameterName == "localPosition"){
					value = transform.localPosition;
					if (context != null && transform.parent == null){
						value = context.InverseTransformPoint( value );
					}
					return value;
				}
				if (parameterName == "localEulerAngles"){
					value = transform.GetLocalEulerAngles();
					if (context != null && transform.parent == null){
						value -= context.GetLocalEulerAngles();
					}
					return value;
				}

				if (parameterName == "localScale"){
					return transform.localScale;
				}
			}

			return isProperty? property.RTGetGetMethod().Invoke(obj, null) : field.GetValue(obj);			
		}

		///Sets the current value to the object
		public void SetCurrentValue(object obj, object value){
			
			if (!isValid){
				return;
			}

			obj = ResolvedObject(obj);
			if (obj == null || obj.Equals(null)){
				return;
			}

			if (obj is Transform){ //special treat
				var transform = obj as Transform;
				var vector = (Vector3)value;
				if (parameterName == "localPosition"){
					if (context != null && transform.parent == null){
						vector = context.TransformPoint( vector );
					}
					transform.localPosition = vector;
					return;
				}

				if (parameterName == "localEulerAngles"){
					if (context != null && transform.parent == null){
						vector += context.GetLocalEulerAngles();
					}
					transform.SetLocalEulerAngles( vector );
					return;
				}

				if (parameterName == "localScale"){
					transform.localScale = vector;
					return;
				}
			}


			if (isProperty){
				property.RTGetSetMethod().Invoke(obj, new object[]{value});
			} else {
				field.SetValue(obj, value);
			}
		}


		///Sets the final evaluated value at time on the target
		public void SetEvaluatedValues(object obj, float time, float previousTime){
			#if UNITY_EDITOR
			if (!Application.isPlaying){
				Internal_SetEvaluatedValues_Editor(obj, time, previousTime);
				return;
			}
			#endif
			Internal_SetEvaluatedValues_Runtime(obj, time, previousTime);
		}


#if UNITY_EDITOR
		////EDITOR EVAL////
		void Internal_SetEvaluatedValues_Editor(object obj, float time, float previousTime){

			if (!enabled || obj == null || obj.Equals(null)){
				return;
			}

			if (!HasAnyKey()){
				#if UNITY_EDITOR //currentEval is checked for HasChanged, so if no keys, nothing is changed.
				if (!Application.isPlaying){
					currentEval = GetCurrentValue(obj);
				}
				#endif
				return;
			}

			#if UNITY_EDITOR 
			if (!Application.isPlaying){
				if (time == previousTime && HasChanged(obj)){
					if (!Prefs.autoKey){ //in case of no Auto-Key, store changed params
						var _value = GetCurrentValue(obj);
						System.Action restore = delegate{ SetCurrentValue(obj, _value); };
						System.Action commit  = delegate{ TryKeyChangedValues(obj, time); };
						var paramCallbacks    = new CutsceneUtility.ChangedParameterCallbacks(restore, commit);
						CutsceneUtility.changedParameterCallbacks[this] = paramCallbacks;
					}
					return; //auto-key or not, return if the parameter changed
				}
				if (!Prefs.autoKey){
					CutsceneUtility.changedParameterCallbacks.Remove(this);
				}
			}
			#endif


			if (animatedType == typeof(bool)){
				currentEval = curve1.Evaluate(time) >= 1? true : false;
			}

			if (animatedType == typeof(int)){
				currentEval = (int)(curve1.Evaluate(time));
			}

			if (animatedType == typeof(float)){
				currentEval = curve1.Evaluate(time);
			}

			if (animatedType == typeof(Vector2)){
				currentEval = new Vector2( curve1.Evaluate(time), curve2.Evaluate(time) );
			}
				
			if (animatedType == typeof(Vector3)){
				currentEval = new Vector3( curve1.Evaluate(time), curve2.Evaluate(time), curve3.Evaluate(time) );
			}

			if (animatedType == typeof(Color)){
				currentEval = new Color( curve1.Evaluate(time), curve2.Evaluate(time), curve3.Evaluate(time), curve4.Evaluate(time) );
			}

			SetCurrentValue(obj, currentEval);
		}
#endif

		////RUNTIME EVAL////
		//The only thing this actually does vs editor evaluation is to save some allocation for animated properties (not fields).
		//Thus, not sure if it's worth the complexity it introduces.
		[NonSerialized] Action<bool> setterBool;
		[NonSerialized] Action<float> setterFloat;
		[NonSerialized] Action<int> setterInt;
		[NonSerialized] Action<Vector2> setterVector2;
		[NonSerialized] Action<Vector3> setterVector3;
		[NonSerialized] Action<Color> setterColor;
		[NonSerialized] object lastResolvedObject;
		void Internal_SetEvaluatedValues_Runtime(object obj, float time, float previousTime){

			if (!enabled || !isValid){
				return;
			}

			if (!HasAnyKey()){
				return;
			}

			obj = ResolvedObject(obj);
			var forceCreateDelegates = lastResolvedObject != obj;
			lastResolvedObject = obj;
			if (obj == null || obj.Equals(null)){
				return;
			}

			if (animatedType == typeof(bool)){
				var value = curve1.Evaluate(time) >= 1? true : false;
				if (isProperty){
					if (setterBool == null || forceCreateDelegates){ setterBool = property.RTGetSetMethod().RTCreateDelegate<Action<bool>>(obj); }
					setterBool(value);
				} else { field.SetValue(obj, value); }
				return;
			}

			if (animatedType == typeof(float)){
				var value = curve1.Evaluate(time);
				if (isProperty){
					if (setterFloat == null || forceCreateDelegates){ setterFloat = property.RTGetSetMethod().RTCreateDelegate<Action<float>>(obj); }
					setterFloat(value);
				} else { field.SetValue(obj, value); }
				return;
			}

			if (animatedType == typeof(int)){
				var value = (int)curve1.Evaluate(time);
				if (isProperty){
					if (setterInt == null || forceCreateDelegates){ setterInt = property.RTGetSetMethod().RTCreateDelegate<Action<int>>(obj); }
					setterInt(value);
				} else { field.SetValue(obj, value); }
				return;
			}

			if (animatedType == typeof(Vector2)){
				var value = new Vector2( curve1.Evaluate(time), curve2.Evaluate(time) );
				if (isProperty){
					if (setterVector2 == null || forceCreateDelegates){ setterVector2 = property.RTGetSetMethod().RTCreateDelegate<Action<Vector2>>(obj); }
					setterVector2(value);
				} else { field.SetValue(obj, value); }
				return;
			}

			if (animatedType == typeof(Vector3)){
				var value = new Vector3( curve1.Evaluate(time), curve2.Evaluate(time), curve3.Evaluate(time) );
				if (obj is Transform){
					var transform = (Transform)obj;
					if (parameterName == "localPosition"){
						if (context != null && transform.parent == null){
							value = context.TransformPoint( value );
						}
						transform.localPosition = value;
						return;
					}

					if (parameterName == "localEulerAngles"){
						if (context != null && transform.parent == null){
							value += context.GetLocalEulerAngles();
						}
						transform.SetLocalEulerAngles( value );
						return;
					}

					if (parameterName == "localScale"){
						transform.localScale = value;
						return;
					}
				}
				if (isProperty){
					if (setterVector3 == null || forceCreateDelegates){ setterVector3 = property.RTGetSetMethod().RTCreateDelegate<Action<Vector3>>(obj); }
					setterVector3(value);
				} else { field.SetValue(obj, value); }
				return;
			}

			if (animatedType == typeof(Color)){
				var value = new Color( curve1.Evaluate(time), curve2.Evaluate(time), curve3.Evaluate(time), curve4.Evaluate(time) );
				if (isProperty){
					if (setterColor == null || forceCreateDelegates){ setterColor = property.RTGetSetMethod().RTCreateDelegate<Action<Color>>(obj); }
					setterColor(value);
				} else { field.SetValue(obj, value); }
				return;
			}
		}
		////////






		///Try to add new key if the value has changed
		public bool TryKeyChangedValues(object obj, float time){

			if (!isValid || !enabled){
				return false;
			}

			if (!HasAnyKey() && !isExternal){
				return false;
			}

			if (HasChanged(obj)){
				SetKeyCurrent(obj, time);
				return true;
			}

			return false;
		}

		///Try add key at time, with identity value either from existing curves or in case of now curves, from current property value.
		public bool TryKeyIdentity(object obj, float time){

			if (!HasAnyKey()){
				SetKeyCurrent(obj, time);
				return true;
			}

			var keyAdded = false;
			foreach(var curve in curves){
				if (AddKey(curve, time, curve.Evaluate(time))){
					keyAdded = true;
				}
			}
			return keyAdded;
		}

		///Has the property on target changed since the last set evaluation?
		public bool HasChanged(object obj){
			var a = currentEval;
			if (a == null){
				return false;
			}
			var b = GetCurrentValue(obj);
			if (b == null){
				return false;
			}

			if (animatedType == typeof(bool)){
				return (bool)a != (bool)b;
			}	
			if (animatedType == typeof(int)){
				return (int)a != (int)b;
			}			
			if (animatedType == typeof(float)){
				return Mathf.Abs( (float)a - (float)b ) > COMPARE_THRESHOLD;
			}
			if (animatedType == typeof(Vector2)){
				return Mathf.Abs( ((Vector2)a - (Vector2)b).magnitude) > COMPARE_THRESHOLD;
			}
			if (animatedType == typeof(Vector3)){
				return Mathf.Abs( ((Vector3)a - (Vector3)b).magnitude) > COMPARE_THRESHOLD;
			}
			if (animatedType == typeof(Color)){
				return Mathf.Abs( ((Vector4)(Color)a - (Vector4)(Color)b).magnitude) > COMPARE_THRESHOLD;
			}
			return !object.Equals(a, b);
		}


		///Sets a key on target at time with it's current property value
		public void SetKeyCurrent(object obj, float time){
			var val = GetCurrentValue(obj);
			SetKey(time, val);
			currentEval = val;
			NotifyChange();
		}

		///Set a key at time and value
		public void SetKey(float time, object value){

			if (!enabled || value == null){
				return;
			}

			if (animatedType == typeof(bool)){
				AddKey(curve1, time, (bool)value? 1 : 0);
				return;
			}

			if (animatedType == typeof(int)){
				AddKey(curve1, time, (int)value);
				return;
			}

			if (animatedType == typeof(float)){
				AddKey(curve1, time, (float)value);
				return;
			}

			if (animatedType == typeof(Vector2)){
				AddKey(curve1, time, ((Vector2)value).x);
				AddKey(curve2, time, ((Vector2)value).y);
				return;
			}

			if (animatedType == typeof(Vector3)){
				AddKey(curve1, time, ((Vector3)value).x);
				AddKey(curve2, time, ((Vector3)value).y);
				AddKey(curve3, time, ((Vector3)value).z);
				return;
			}

			if (animatedType == typeof(Color)){
				AddKey(curve1, time, ((Color)value).r);
				AddKey(curve2, time, ((Color)value).g);
				AddKey(curve3, time, ((Color)value).b);
				AddKey(curve4, time, ((Color)value).a);
				return;
			}
		}


		///Add key at time - value
		bool AddKey(AnimationCurve curve, float time, float value){

			time = Mathf.Max(time, 0);

			for (var i = 0; i < curve.keys.Length; i++){
				if ( Mathf.Abs( curve.keys[i].time - time) < PROXIMITY_TOLERANCE ){
					var key = curve.keys[i];
					key.time = time;
					key.value = value;
					curve.MoveKey(i, key);
					curve.UpdateTangentsFromMode();
					return false;
				}
			}

			if (animatedType == typeof(bool) || animatedType == typeof(int)){
				curve.AddKey(KeyframeUtility.GetNew(time, value, TangentMode.Constant));
			} else {
				
				#if UNITY_EDITOR
				if (Prefs.defaultTangentMode != TangentMode.Editable){
					curve.AddKey( KeyframeUtility.GetNew(time, value, Prefs.defaultTangentMode) );
				} else {
					curve.AddKey(time, value);
				}
				#else
				curve.AddKey(time, value);
				#endif
			}
			curve.UpdateTangentsFromMode();

			NotifyChange();

			return true;
		}

		///Remove keys at time
		public void RemoveKey(float time){
			foreach(var curve in curves){
				for (var i = 0; i < curve.keys.Length; i++){
					var key = curve.keys[i];
					if ( Mathf.Abs(key.time - time) < PROXIMITY_TOLERANCE ){
						curve.RemoveKey(i);
						break;
					}
				}

				curve.UpdateTangentsFromMode();
			}

			NotifyChange();
		}

		///Set curves PreWrap
		public void SetPreWrapMode(WrapMode mode){
			foreach(var curve in curves){
				curve.preWrapMode = mode;
			}

			NotifyChange();
		}
	
		///Set curves PostWrap
		public void SetPostWrapMode(WrapMode mode){
			foreach(var curve in curves){
				curve.postWrapMode = mode;
			}

			NotifyChange();
		}

		///Are there any keys at all?
		public bool HasAnyKey(){
			for (var i = 0; i < curves.Length; i++){
				if (curves[i].length > 0){
					return true;
				}
			}
			return false;
		}

		///Has any key at time?
		public bool HasKey(float time){
			if (time >= 0){
				for (var i = 0; i < curves.Length; i++){
					if (curves[i].keys.Any( k => Mathf.Abs(k.time - time) < PROXIMITY_TOLERANCE )){
						return true;
					}
				}
			}
			return false;
		}

		///Returns the key time after time, or first if time is last key time.
		public float GetKeyNext(float time){
			var keys = new List<Keyframe>();
			foreach(var curve in curves){
				keys.AddRange(curve.keys);
			}
			keys = keys.OrderBy(k => k.time).ToList();
			var key = keys.FirstOrDefault(k => k.time > time + COMPARE_THRESHOLD);
			return key.time == 0 && !HasKey(0)? keys.FirstOrDefault().time : key.time;
		}

		///Returns the key time before time, or last if time is first key time.
		public float GetKeyPrevious(float time){
			var keys = new List<Keyframe>();
			foreach(var curve in curves){
				keys.AddRange(curve.keys);
			}
			keys = keys.OrderBy(k => k.time).ToList();
			if (time == 0){ return keys.LastOrDefault().time; }
			var key = keys.LastOrDefault(k => k.time < time - COMPARE_THRESHOLD);
			return key.time == 0 && !HasKey(0)? keys.LastOrDefault().time : key.time;
		}


		///Offset all curves by value.
		public void OffsetValue(object byValue){
			
			if (animatedType == typeof(float)){
				OffsetCurveValues(curve1, (float)byValue);
			}

			if (animatedType == typeof(int)){
				OffsetCurveValues(curve1, (int)byValue);
			}

			if (animatedType == typeof(Vector2)){
				OffsetCurveValues(curve1, ((Vector2)byValue).x);
				OffsetCurveValues(curve2, ((Vector2)byValue).y);
			}

			if (animatedType == typeof(Vector3)){
				OffsetCurveValues(curve1, ((Vector3)byValue).x);
				OffsetCurveValues(curve2, ((Vector3)byValue).y);
				OffsetCurveValues(curve3, ((Vector3)byValue).z);
			}

			if (animatedType == typeof(Color)){
				OffsetCurveValues(curve1, ((Color)byValue).r);
				OffsetCurveValues(curve2, ((Color)byValue).g);
				OffsetCurveValues(curve3, ((Color)byValue).b);
				OffsetCurveValues(curve4, ((Color)byValue).a);
			}

			NotifyChange();
		}

		private void OffsetCurveValues(AnimationCurve curve, float byValue){
			for (var i = 0; i < curve.keys.Length; i++){
				var key = curve.keys[i];
				key.value += byValue;
				curve.MoveKey(i, key);
			}
		}



		///Reset curves
		public void Reset(){
			foreach(var curve in curves){
				curve.preWrapMode = WrapMode.Once;
				curve.postWrapMode = WrapMode.Once;
				curve.keys = new Keyframe[0];
			}

			NotifyChange();
		}

		///Changes the type of the parameter (field/property) while keeping the data
		public void ChangeMemberType(ParameterType newType){
			if (newType != this.parameterType){
				data.parameterType = newType;
				serializedData = JsonUtility.ToJson(data);
			}
		}


		///A friendly label included all info
		public override string ToString(){
			
			if (string.IsNullOrEmpty(serializedData)){
				return "NOT SET!";
			}

			if ( !isValid ){
				return string.Format("*{0}*", data.parameterName);
			}
			
			var name = parameterName;
			if (name == "localPosition") name = "Position";
			if (name == "localEulerAngles") name = "Rotation";
			if (name == "localScale") name = "Scale";
			if (isExternal){
				name = string.Format("{0} <i>({1})</i>", name, declaringType.Name);
			}
			if (!enabled){
				name += " <i>(Disabled)</i>";
			}
			return string.IsNullOrEmpty(transformHierarchyPath)? name.SplitCamelCase() : transformHierarchyPath + "/" + name.SplitCamelCase();
		}

		///Returns formated text of value at time
		public string GetKeyLabel(float time){
			
			var label = string.Empty;

			if (animatedType == typeof(bool)){
				label = curve1.Evaluate(time) >= 1? "true" : "false";
			}

			if (animatedType == typeof(float)){
				label = curve1.Evaluate(time).ToString("0.0");
			}

			if (animatedType == typeof(int)){
				label = curve1.Evaluate(time).ToString("0");
			}

			if (animatedType == typeof(Vector2) || animatedType == typeof(Vector3)){
				for (var i = 0; i < curves.Length; i++){
					label += curves[i].Evaluate(time).ToString("0");
					if (i < curves.Length-1){ label += ","; }
				}								
			}

			if (animatedType == typeof(Color)){
				var color = new Color(curve1.Evaluate(time), curve2.Evaluate(time), curve3.Evaluate(time), curve4.Evaluate(time));
				return string.Format("<color={0}><size=14>●</size></color>", ColorToHex(color));
			}

			return string.Format("({0})", label);
		}

		//this is only used in GetKeyLabel above to display color instead of numerical values
		private string ColorToHex(Color32 color){
			return ("#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2")).ToLower();
		}


		//notify the dopesheet to refresh rendered keys
		void NotifyChange(){
			#if UNITY_EDITOR
			DopeSheetEditor.RefreshDopeKeysOf(this);
			#endif
		}

	}
}