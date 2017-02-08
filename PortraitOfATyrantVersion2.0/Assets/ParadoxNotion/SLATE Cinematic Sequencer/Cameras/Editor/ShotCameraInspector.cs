#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Slate{

	[CustomEditor(typeof(ShotCamera))]
	public class ShotCameraInspector : Editor {

		private bool lookThrough;
		private Vector3 lastPos;
		private Quaternion lastRot;

		private ShotCamera shot{
			get {return target as ShotCamera;}
		}

		public override void OnInspectorGUI(){

			EditorGUILayout.HelpBox("The Camera Component attached above is mostly used for Editor Previews. Changing the Camera Component settings above has no effect.\nYou can instead change the 'Render Camera' settings if so required found under the 'Director Camera Root' GameObject, and which is the only Camera Cutscenes are rendered from within.", MessageType.Info);

			GUI.backgroundColor = lookThrough? new Color(1,0.4f,0.4f) : Color.white;
			if (GUILayout.Button(lookThrough? "Stop Adjusting" : "Adjust Transforms With Scene View Camera")){
				lookThrough = !lookThrough;
			}
			GUI.backgroundColor = Color.white;

			shot.fieldOfView = EditorGUILayout.Slider("Field Of View", shot.fieldOfView, 5, 170);
			shot.focalPoint = Mathf.Max( EditorGUILayout.FloatField("Focal Point", shot.focalPoint), 0 ) ;
			shot.focalRange = Mathf.Max( EditorGUILayout.FloatField("Focal Range", shot.focalRange), 0 ) ;
			if (GUI.changed){
				EditorUtility.SetDirty(shot);
			}
		}

		void OnSceneGUI(){

			Handles.color = Prefs.gizmosColor;
			Handles.Label(shot.position + new Vector3(0,0.5f,0), shot.gameObject.name);
			Handles.color = Color.white;

			var sc = SceneView.lastActiveSceneView;
			if (lookThrough && sc != null){

				if (sc.camera.transform.position != lastPos || sc.camera.transform.rotation != lastRot){
					shot.position = sc.camera.transform.position;
					shot.rotation = sc.camera.transform.rotation;
					EditorUtility.SetDirty(shot.gameObject);
				}

				lastPos = sc.camera.transform.position;
				lastRot = sc.camera.transform.rotation;
			}
		}
	}
}

#endif