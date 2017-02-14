using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InventoryLibrary {


	static InventoryObject[] list;

	static InventoryLibrary(){
		list = Resources.LoadAll<InventoryObject>("Prefabs/Inventory Objects");
	}

	public static InventoryObject GetObject(string objectName){
		for (int i = 0; i < list.Length; i++) {
			var o = list[i];
			if (o.name == objectName) {
				return o;
			}
		}
		Debug.LogError("Can't find InventoryObject " + objectName);
		return null;
	}

}
