﻿#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Slate{

	[CustomEditor(typeof(CameraShot))]
	public class CameraShotInspector : ActionClipInspector<CameraShot> {

		SerializedProperty blendInEffectProp;
		SerializedProperty blendOutEffectProp;
		SerializedProperty steadyCamEffectProp;
		SerializedProperty shotAnimModeProp;
		SerializedProperty externalClipProp;

		void OnEnable(){

			blendInEffectProp = serializedObject.FindProperty("blendInEffect");
			blendOutEffectProp = serializedObject.FindProperty("blendOutEffect");
			steadyCamEffectProp = serializedObject.FindProperty("steadyCamEffect");
			shotAnimModeProp = serializedObject.FindProperty("shotAnimationMode");
			externalClipProp = serializedObject.FindProperty("externalAnimationClip");

			action.lookThrough = false;
		}

		void OnDisable(){
			action.lookThrough = false;
		}

		public override void OnInspectorGUI(){
			
			base.ShowCommonInspector();


			serializedObject.Update();

			EditorGUILayout.PropertyField(blendInEffectProp);
			if (action.parent.children.OfType<CameraShot>().FirstOrDefault() == action){
				if (action.blendInEffect == CameraShot.BlendInEffectType.EaseIn){
					EditorGUILayout.HelpBox("The 'Ease In' option has no effect in the first shot clip of the track.", MessageType.Warning);
				}
				if (action.blendInEffect == CameraShot.BlendInEffectType.CrossDissolve){
					EditorGUILayout.HelpBox("The 'Cross Dissolve' option has no usable effect in the first shot clip of the track.", MessageType.Warning);
				}
			}

			EditorGUILayout.PropertyField(blendOutEffectProp);
			EditorGUILayout.PropertyField(steadyCamEffectProp);

			EditorGUILayout.PropertyField(shotAnimModeProp);
			if (action.shotAnimationMode == CameraShot.ShotAnimationMode.UseExternalAnimationClip){
				EditorGUILayout.PropertyField(externalClipProp);
			}

			serializedObject.ApplyModifiedProperties();
			if (GUI.changed){
				action.Validate();
			}


			if (GUILayout.Button(action.targetShot == null? "Select Shot" : "Replace Shot")){
				if (action.targetShot == null || EditorUtility.DisplayDialog("Replace Shot", "Selecting a new target shot will reset all animation data of this clip.", "OK", "Cancel")){
					ShotPicker.Show(Event.current.mousePosition, action.root, (shot)=> { action.targetShot = shot; } );
				}
			}

			if (action.targetShot == null && GUILayout.Button("Create Shot")){
				action.targetShot = ShotCamera.Create(action.root.context.transform);
			}

			if (action.targetShot != null){

				if (GUILayout.Button("Find in Scene")){
					Selection.activeGameObject = action.targetShot.gameObject;
				}

				var lastRect = GUILayoutUtility.GetLastRect();
				var rect = new Rect(lastRect.x, lastRect.yMax + 5, lastRect.width, 200);
				
				var res = EditorTools.GetGameViewSize();
				var texture = action.targetShot.GetRenderTexture( (int)res.x, (int)res.y );
				var style = new GUIStyle("Box");
				style.alignment = TextAnchor.MiddleCenter;
				GUI.Box(rect, texture, style);

				GUILayout.Space(205);

				var helpRect = new Rect(rect.x + 10, rect.yMax - 20, rect.width - 20, 16);
				GUI.color = EditorGUIUtility.isProSkin? new Color(0,0,0,0.6f) : new Color(1,1,1,0.6f);
				GUI.DrawTexture(helpRect, Slate.Styles.whiteTexture);
				GUI.color = Color.white;
				GUI.Label(helpRect, "Left: Rotate, Middle: Pan, Right: Dolly, Alt+Right: Zoom");

				var e = Event.current;
				if (rect.Contains(e.mousePosition)){
					EditorGUIUtility.AddCursorRect(rect, MouseCursor.Pan);
					if (e.type == EventType.MouseDrag){

						Undo.RecordObject(action.targetShot.transform, "Shot Change");
						Undo.RecordObject(action.targetShot.cam, "Shot Change");
						Undo.RecordObject(action.targetShot, "Shot Change");

						var in2DMode = false;
						var sc = UnityEditor.SceneView.lastActiveSceneView;
						if (sc != null){
							in2DMode = sc.in2DMode;
						}

						//look
						if (e.button == 0 && !in2DMode){
							var deltaRot = new Vector3(e.delta.y, e.delta.x, 0) * 0.5f;
							action.targetShot.localEulerAngles += deltaRot;
							e.Use();
						}
						//pan
						if (e.button == 2 || (e.button == 0 && in2DMode) ){
							var deltaPos = new Vector3(-e.delta.x, e.delta.y, 0) * (e.shift? 0.01f : 0.05f);
							action.targetShot.transform.Translate(deltaPos);
							e.Use();
						}
						//dolly in/out
						if (e.button == 1 && !e.alt){
							action.targetShot.transform.Translate(0, 0, e.delta.x * 0.05f);
							e.Use();
						}
						//fov
						if (e.button == 1 && e.alt){
							action.fieldOfView -= e.delta.x;
							e.Use();
						}

						EditorUtility.SetDirty(action.targetShot.transform);
						EditorUtility.SetDirty(action.targetShot.cam);
						EditorUtility.SetDirty(action.targetShot);
					}
				}

				base.ShowAnimatableParameters();
			}
		}
	}
}

#endif