using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomPropertyDrawer(typeof(DialogueNode.DialogueLine))]
//public class DialogueLineDrawer : PropertyDrawer {
//	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
//	{
//		var contentPos = EditorGUI.PrefixLabel(position, label);
//		contentPos.width = .75f;
//		contentPos.height = 16;
//		EditorGUI.PropertyField(position, property.FindPropertyRelative("audioClip"), GUIContent.none);
//		EditorGUI.PropertyField(pos, txt, GUIContent.none);
//	}
//	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
//		return (50f);
//	}
//}
//
/*[CustomEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : Editor{
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("nextNode"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("character"));

		var list = serializedObject.FindProperty("lines");
		EditorGUILayout.PropertyField(list);

		EditorGUI.indentLevel+=1;

		EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
		for(int i = 0; i < list.arraySize; i++){
			var prop = list.GetArrayElementAtIndex(i);
			EditorGUILayout.PropertyField(prop.FindPropertyRelative("text"), GUIContent.none, true);
			EditorGUILayout.PropertyField(prop.FindPropertyRelative("audioClip"), GUIContent.none, true);

		}

		EditorGUI.indentLevel-=1;

		serializedObject.ApplyModifiedProperties();
	}

}*/
