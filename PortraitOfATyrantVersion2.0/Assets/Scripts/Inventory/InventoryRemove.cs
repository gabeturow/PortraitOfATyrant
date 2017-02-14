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
		Debug.Log("RemoveThingNow");
		InventoryMan.main.RemoveRender(ItemName);
		//pickUp.pickups.Remove(this.Identifier);
		if(InventoryObj!=null){
			InventoryMan.main.Remove(InventoryObj);
				}

	}

//	void Update(){
//		Debug.Log(InventoryMan.main.selectedObject.name);
//	}
		
		
}
