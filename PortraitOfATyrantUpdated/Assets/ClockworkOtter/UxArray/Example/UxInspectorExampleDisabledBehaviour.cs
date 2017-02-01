/* Copyright 2013-2014 Tom Ketola */
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

[DontUseUxInspector]
public class UxInspectorExampleDisabledBehaviour: MonoBehaviour
{
	[UxInspectInfo("This is a regular string")]
	public string HereIsARegularString;
	[UxInspectInfo("This is a list of strings")]
	public List<string> ListOfStrings = new List<string>();
	[UxInspectInfo("This is a list of strings")]
	public List<GameObject> ListOfObjects = new List<GameObject>();

	[System.Serializable]	
	[UseUxInspector]
	public class NestedStruct
	{
		[UxInspectInfo("This is a nested object")]
		public List<Vector3> anotherExample = new List<Vector3>();
	}

	//This list is public, but not visible
	public List<string> PublicListNotVisible = new List<string>();
	[UxInspectInfo("This is a regular string")]
	public string _HereIsAnotherRegularString;
	[UxInspectInfo("blahblah")]
	public List<NestedStruct> structList = new List<NestedStruct>();
	[UxInspectInfo("Monobehaviour list")]
	public List<GameObject> behaviourList = new List<GameObject>();
}
