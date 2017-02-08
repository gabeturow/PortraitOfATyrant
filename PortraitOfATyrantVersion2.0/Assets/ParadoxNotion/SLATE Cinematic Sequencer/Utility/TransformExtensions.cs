using UnityEngine;
using System.Reflection;
using System;

namespace Slate{

	public static class TransformExtensions {

		public static Vector3 GetLocalEulerAngles(this Transform transform){

			if (Application.isPlaying){
				return transform.localEulerAngles;
			}
			
			#if UNITY_EDITOR && UNITY_5_4_OR_NEWER

			var t = typeof(Transform);
			var m = t.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
			var p = t.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
			return (Vector3)m.Invoke(transform, new object[]{ p.GetValue(transform, null) });
			
			#else

			return transform.localEulerAngles;

			#endif
		}

		public static void SetLocalEulerAngles(this Transform transform, Vector3 value){
			
			if (Application.isPlaying){
				transform.localEulerAngles = value;
				return;
			}

			#if UNITY_EDITOR && UNITY_5_4_OR_NEWER
			
			var t = typeof(Transform);
			var m = t.GetMethod("SetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
			var p = t.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
			m.Invoke(transform, new object[]{ value, p.GetValue(transform, null) });

			#else

			transform.localEulerAngles = value;

			#endif
		}

		///Gets existing T component or adds new one if not exists
		public static T GetAddComponent<T>(this GameObject go) where T:Component{
			T result = go.GetComponent<T>();
			if (result == null){
				result = go.AddComponent<T>();
			}
			return result;			
		}

		///Gets existing T component or adds new one if not exists
		public static T GetAddComponent<T>(this Component comp) where T:Component{
			T result = comp.GetComponent<T>();
			if (result == null){
				result = comp.gameObject.AddComponent<T>();
			}
			return result;			
		}
	}
}