using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(OtterListTest2))]
public class OtterListTest2Inspector : Editor
{

	public override void OnInspectorGUI()
	{
		//OtterListTest2 ottherListTest2 = target as OtterListTest2;
		SerializedProperty property = serializedObject.GetIterator();
		property.Next(true);	//Get the first
		GUILayout.BeginVertical();
		while (property.NextVisible(false))
		{
			if (property.name == "m_CustomName"
				|| property.name == "m_Script")
				continue;

			DrawProperty(property);
		}
		GUILayout.EndVertical();
	}

	protected void DrawProperty(SerializedProperty property)
	{
		GUIContent content = new GUIContent();
		content.text = ObjectNames.NicifyVariableName(property.name);

		EditorGUILayout.PropertyField(property, content, true);
	}
}
