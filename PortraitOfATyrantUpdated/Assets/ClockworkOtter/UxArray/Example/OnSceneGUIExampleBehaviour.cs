/* Copyright 2013-2014 Tom Ketola */
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(OrdinaryListExampleBehaviour), true)]
public class OnSceneGUIExampleBehaviourEditor : Editor
{	
	static int NumCalls = 0;

	void OnSceneGUI()
	{
		NumCalls++;
		//Debug.Log ("This should get called a bunch");
	}
}
#endif