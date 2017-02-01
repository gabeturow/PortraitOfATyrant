/* Copyright 2013-2014 Tom Ketola */
using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

[UseUxInspector]
public class OrdinaryListDerivedExplicitOverrideExampleBehaviour: OrdinaryListExampleBehaviour
{
	public string derivedString = "Hello";
}