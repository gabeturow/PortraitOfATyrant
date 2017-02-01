//=============================================================================
// ListSeriablizer
// Copyright Pigasus Games
// Author Petrucio
//
// This just shows sample usages of the ListSerializer scripts
//=============================================================================

using UnityEngine;
using System.Collections;

//=============================================================================
[ExecuteInEditMode]
public class SampleUsage : MonoBehaviour {
	
	//=============================================================================
	// Simple usage:
	public string[] myStringList;
	
	void SimpleImport(string[] values) {
		this.myStringList = values;
	}
	void SimpleExport(Pigasus.ListSerializer serializer) {
		serializer.Export<string>(this.myStringList);
	}
	
	//=============================================================================
	// Using with our own more complex types:
	[System.Serializable]
	public class MyType {
		public string title;
		public float  a_float;
		public Vector3[] values;
	}
	public MyType[] myTypeList;
	
	void MyTypeImport(MyType[] values) {
		this.myTypeList = values;
	}
	void MyTypeExport(Pigasus.ListSerializer serializer) {
		serializer.Export<MyType>(this.myTypeList);
	}
	
}

