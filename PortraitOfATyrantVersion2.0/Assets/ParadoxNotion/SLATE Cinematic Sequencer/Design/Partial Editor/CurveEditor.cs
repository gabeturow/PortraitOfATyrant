#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace Slate{

	///A curve editor and renderer using Unity's native one by reflection
	public static class CurveEditor {

		private static Dictionary<IAnimatableData, CurveRenderer> cache = new Dictionary<IAnimatableData, CurveRenderer>();
		public static void DrawCurves(IAnimatableData animatable, IKeyable keyable, Rect posRect, Rect timeRect){
			CurveRenderer instance = null;
			if (!cache.TryGetValue(animatable, out instance)){
				cache[animatable] = instance = new CurveRenderer(animatable, keyable, posRect);
			}
			instance.Draw(posRect, timeRect);
		}

		public static void FrameAllCurves(IAnimatableData animatable){
			CurveRenderer instance = null;
			if (!cache.TryGetValue(animatable, out instance)){
				return;
			}
			instance.FrameAllCurves();
		}


		class CurveRenderer{

			public IAnimatableData animatable;
			public IKeyable keyable;

			private AnimationCurve[] curves;
			private Rect posRect;
			private Rect timeRect;

			private System.Type cEditorType;
			private object cEditor;
			private Dictionary<AnimationCurve, Action<WrapMode, WrapMode>> curvesWrapSetters;

			public CurveRenderer(IAnimatableData animatable, IKeyable keyable, Rect posRect){
				this.animatable = animatable;
				this.keyable = keyable;
				this.curves = animatable.GetCurves();
				this.posRect = posRect;
				Init();
			}

			public void Init(){
				var editorAssembly = typeof(Editor).Assembly;
				cEditorType = editorAssembly.GetType("UnityEditor.CurveEditor");
				var cRendererType = editorAssembly.GetType("UnityEditor.NormalCurveRenderer");
				var cWrapperType = editorAssembly.GetType("UnityEditor.CurveWrapper");
				var cEditorCTR = cEditorType.GetConstructor(new System.Type[]{typeof(Rect), cWrapperType.MakeArrayType(), typeof(bool)});

				
				var arr = System.Array.CreateInstance(cWrapperType, curves.Length);
				curvesWrapSetters = new Dictionary<AnimationCurve, Action<WrapMode, WrapMode>>();
				for (int i = 0; i < curves.Length; i++){
					var cRenderer = System.Activator.CreateInstance(cRendererType, curves[i]);
					var cWrapper = System.Activator.CreateInstance(cWrapperType);
					var cWrapperClr = cWrapperType.GetField("color");
					var cWrapperID = cWrapperType.GetField("id");
					
					var clr = Color.white;
					if (i == 0) clr = Color.red;
					if (i == 1) clr = Color.green;
					if (i == 2) clr = Color.blue;
					cWrapperClr.SetValue(cWrapper, clr);
					cWrapperID.SetValue(cWrapper, i);
					cWrapperType.GetProperty("renderer").SetValue(cWrapper, cRenderer, null);

					
					var wrapSetter = cRendererType.GetMethod("SetWrap", new Type[]{ typeof(WrapMode), typeof(WrapMode) }).RTCreateDelegate<Action<WrapMode, WrapMode>>(cRenderer);
					curvesWrapSetters[curves[i]] = wrapSetter;


					(arr as System.Array).SetValue(cWrapper, i);
				}

				cEditor = cEditorCTR.Invoke(new object[]{ posRect, arr, true } );

				hRangeLocked = true;
				vRangeLocked = false;
				hRangeMin = 0;
				hSlider = false;
				vSlider = true;
				invSnap = 1f/Prefs.snapInterval;
				lastSnapPref = Prefs.snapInterval;
				ignoreScrollWheelUntilClicked = true;

				CreateDelegates();

				RecalculateBounds();
				FrameClip(true, true);
			}

			private Action onGUI;
			private float lastSnapPref;

			private Action<Rect> rectSetter;
			private Func<Rect> rectGetter;
			
			private Action<Rect> shownAreaSetter;
			private Func<Rect> shownAreaGetter;

			private Action<float> hRangeMaxSetter;
			private Func<float> hRangeMaxGetter;


			//create delegates for some properties and methods for performance
			void CreateDelegates(){

				onGUI = cEditorType.GetMethod("OnGUI").RTCreateDelegate<Action>(cEditor);

				rectSetter = cEditorType.GetProperty("rect").GetSetMethod().RTCreateDelegate<Action<Rect>>(cEditor);
				rectGetter = cEditorType.GetProperty("rect").GetGetMethod().RTCreateDelegate<Func<Rect>>(cEditor);

				shownAreaSetter = cEditorType.GetProperty("shownArea").GetSetMethod().RTCreateDelegate<Action<Rect>>(cEditor);
				shownAreaGetter = cEditorType.GetProperty("shownArea").GetGetMethod().RTCreateDelegate<Func<Rect>>(cEditor);

				hRangeMaxSetter = cEditorType.GetProperty("hRangeMax").GetSetMethod().RTCreateDelegate<Action<float>>(cEditor);
				hRangeMaxGetter = cEditorType.GetProperty("hRangeMax").GetGetMethod().RTCreateDelegate<Func<float>>(cEditor);


				var field = cEditorType.GetField("curvesUpdated");
				var methodInfo = typeof(Slate.CurveEditor.CurveRenderer).GetMethod("OnCurvesUpdated", BindingFlags.Instance | BindingFlags.NonPublic);
				Delegate handler = Delegate.CreateDelegate(field.FieldType, this, methodInfo);
				field.SetValue(cEditor, handler);
			}


			public Rect rect{
				get {return rectGetter();}
				set {rectSetter(value);}
			}

			public Rect shownArea{
				get {return shownAreaGetter();}
				set {shownAreaSetter(value);}
			}

			public Rect shownAreaInsideMargins{
				get {return (Rect)cEditorType.GetProperty("shownAreaInsideMargins").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("shownAreaInsideMargins").SetValue(cEditor, value, null);}
			}

			public bool enableMouseInput{
				get {return (bool)cEditorType.GetProperty("enableMouseInput").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("enableMouseInput").SetValue(cEditor, value, null);}
			}

			public bool ignoreScrollWheelUntilClicked{
				get {return (bool)cEditorType.GetProperty("ignoreScrollWheelUntilClicked").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("ignoreScrollWheelUntilClicked").SetValue(cEditor, value, null);}
			}

			public bool hRangeLocked{
				get {return (bool)cEditorType.GetProperty("hRangeLocked").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("hRangeLocked").SetValue(cEditor, value, null);}
			}

			public bool vRangeLocked{
				get {return (bool)cEditorType.GetProperty("vRangeLocked").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("vRangeLocked").SetValue(cEditor, value, null);}
			}

			public bool hSlider{
				get {return (bool)cEditorType.GetProperty("hSlider").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("hSlider").SetValue(cEditor, value, null);}
			}

			public bool vSlider{
				get {return (bool)cEditorType.GetProperty("vSlider").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("vSlider").SetValue(cEditor, value, null);}
			}

			public float hRangeMin{
				get {return (float)cEditorType.GetProperty("hRangeMin").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("hRangeMin").SetValue(cEditor, value, null);}
			}

			public float hRangeMax{
				get {return hRangeMaxGetter();}
				set {hRangeMaxSetter(value);}
			}


			public bool hAllowExceedBaseRangeMin{
				get {return (bool)cEditorType.GetProperty("hAllowExceedBaseRangeMin").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("hAllowExceedBaseRangeMin").SetValue(cEditor, value, null);}								
			}


			public float vRangeMin{
				get {return (float)cEditorType.GetProperty("vRangeMin").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("vRangeMin").SetValue(cEditor, value, null);}
			}

			public float vRangeMax{
				get {return (float)cEditorType.GetProperty("vRangeMax").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("vRangeMax").SetValue(cEditor, value, null);}
			}

			public float invSnap{
				get {return (float)cEditorType.GetField("invSnap").GetValue(cEditor);}
				set {cEditorType.GetField("invSnap").SetValue(cEditor, value);}
			}

			public int axisLock{
				set {cEditorType.GetField("m_AxisLock", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(cEditor, value);}
			}

			public bool hasSelection{
				get {return (bool)cEditorType.GetProperty("hasSelection").GetValue(cEditor, null);}
				set {cEditorType.GetProperty("hasSelection").SetValue(cEditor, value, null);}
			}

			public void FrameSelected(bool h, bool v){
				cEditorType.GetMethod("FrameSelected").Invoke(cEditor, new object[]{h, v});
			}

			public void FrameClip(bool h, bool v){
				#if UNITY_5_4_OR_NEWER
				cEditorType.GetMethod("FrameClip").Invoke(cEditor, new object[]{h, v});
				#else
				FrameSelected(true, true);
				#endif
			}

			public void SelectNone(){
				cEditorType.GetMethod("SelectNone").Invoke(cEditor, null);
			}

			public void RecalculateBounds(){
				cEditorType.GetMethod("RecalculateBounds").Invoke(cEditor, null);
			}

			public void SetShownVRangeInsideMargins(float min, float max){
				cEditorType.GetMethod("SetShownVRangeInsideMargins").Invoke(cEditor, new object[]{min, max});
			}

			
			public void OnGUI(){
				try { onGUI(); }
				catch (Exception e){ SelectNone(); Debug.LogError(e.Message); }
			}

			public void DrawCurves(){
				cEditorType.GetMethod("DrawCurves").Invoke(cEditor, null);
			}

			public void FrameAllCurves(){
				RecalculateBounds();
				FrameClip(true, true);
			}

			public void Draw(Rect posRect, Rect timeRect){

				if (curves == null || curves.Length == 0){
					GUI.Label(posRect, "No Animation Curves to Display", Styles.centerLabel);
					return;
				}

				if (Prefs.snapInterval != lastSnapPref){
					lastSnapPref = Prefs.snapInterval;
					invSnap = 1/Prefs.snapInterval;
				}

				for (var i = 0; i < curves.Length; i++){
					var c = curves[i];
					curvesWrapSetters[c](c.preWrapMode, c.postWrapMode);
				}


				hRangeMax = timeRect.xMax;
				rect = posRect;


				var e = Event.current;
				if (Prefs.lockHorizontalCurveEditing && e.rawType == EventType.MouseDrag){
					axisLock = 2;
				}

				if (e.rawType == EventType.MouseUp){
					RecalculateBounds();
					if (GUIUtility.hotControl != 0){
						OnCurvesUpdated();
					}
				}

				if ( (e.type == EventType.MouseDown && e.button == 0 && e.clickCount == 2 && posRect.Contains(e.mousePosition)) || (e.control && e.keyCode == KeyCode.F) ){
					FrameAllCurves();
					e.Use();
				}

				GUI.color = new Color(1,1,1,0.2f);
				var infoWidth = Mathf.Min(posRect.width, 125);
				var labelRect = new Rect(posRect.xMax - infoWidth - 10, posRect.y + 2, infoWidth, 18);
				GUI.Label(labelRect, "(F: Frame Selection)");
				#if UNITY_5_4_OR_NEWER
				labelRect.y += 18;
				GUI.Label(labelRect, "(Ctrl+F: Frame All)");
				#endif
				labelRect.y += 18;
				GUI.Label(labelRect, "(Alt+: Pan/Zoom)");
				GUI.color = Color.white;

				shownArea = Rect.MinMaxRect(timeRect.xMin, shownArea.yMin, timeRect.xMax, shownArea.yMax);

				OnGUI();
			}


			//notify Dopesheet editor
			void OnCurvesUpdated(){
				DopeSheetEditor.RefreshDopeKeysOf(animatable);
			}
		}
	}
}

#endif