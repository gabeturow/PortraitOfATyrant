#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Slate{

	[CustomEditor(typeof(DirectorCamera))]
	public class DirectorCameraInspector : Editor {

		public override void OnInspectorGUI(){
			base.OnInspectorGUI();
			EditorGUILayout.HelpBox("This is the master Director Camera Root. The child 'Render Camera' is from within where all cutscenes are rendered from. You can add any Image Effects in that Camera and even animate them if so required by using a Properties Track in the Director Group.", MessageType.Info);

			GUILayout.Space(10);
			DirectorCamera.matchMainCamera = EditorGUILayout.Toggle("Match Main When Active", DirectorCamera.matchMainCamera);
			EditorGUILayout.HelpBox("If true, will copy the Main Camera settings to Render Camera when it becomes active.\nIf false, the Render Camera settings will stay intact.", MessageType.None);
			
			GUILayout.Space(10);
			DirectorCamera.setMainWhenActive = EditorGUILayout.Toggle("Set Main When Active", DirectorCamera.setMainWhenActive);
			EditorGUILayout.HelpBox("If true, will set the Render Camera as MainCamera tag (Camera.main) for the duration of cutscenes.\nIf false, tags will remain intact.", MessageType.None);
			
			GUILayout.Space(10);
			DirectorCamera.dontDestroyOnLoad = EditorGUILayout.Toggle("Dont Destroy On Load", DirectorCamera.dontDestroyOnLoad);
			EditorGUILayout.HelpBox("If true, will make this Director Camera instance persist when loading a new level.\nIf false, the DirectorCamera of the new level will be used.", MessageType.None);
		}
	}
}

#endif