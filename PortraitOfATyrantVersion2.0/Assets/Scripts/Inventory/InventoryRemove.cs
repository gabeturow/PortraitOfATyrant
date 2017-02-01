using UnityEngine;
using System.Collections;

public class InventoryRemove : InteractionModule {


	[SerializeField]
	string ItemName;
	public InventoryObject InventoryObj;

	PickupModule pickUp;

	public override void OnInteract ()
	{
		pickUp=GetComponent<PickupModule>();
		base.OnInteract ();

		InventoryMan.main.RemoveRender(ItemName);
//		pickUp.pickups.Remove(ItemName);
		if(InventoryObj!=null){
		//	InventoryMan.main.Remove(InventoryObj);
				}

	}

//	void Update(){
//		Debug.Log(InventoryMan.main.selectedObject.name);
//	}
		
		
}
