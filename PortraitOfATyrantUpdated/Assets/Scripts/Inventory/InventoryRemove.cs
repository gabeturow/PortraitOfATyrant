using UnityEngine;
using System.Collections;

public class InventoryRemove : InteractionModule {


	[SerializeField]
	string ItemName;

	public override void OnInteract ()
	{
		base.OnInteract ();

		InventoryMan.main.RemoveRender(ItemName);


	}

//	void Update(){
//		Debug.Log(InventoryMan.main.selectedObject.name);
//	}
		
		
}
