/* Copyright 2013-2014 Tom Ketola */
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public class OrdinaryListExampleBehaviour: MonoBehaviour
{
	public string HereIsARegularString;
	public List<string> ListOfStrings = new List<string>();

	
	public List<string> PublicListNumberTwo = new List<string>();
	public string _HereIsAnotherRegularString;
	[UxInspectInfo("This is a list of objects")]
	public List<GameObject> ListOfObjects = new List<GameObject>();
}