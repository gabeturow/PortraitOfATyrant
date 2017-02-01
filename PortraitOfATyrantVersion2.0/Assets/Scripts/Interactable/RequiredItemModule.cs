using UnityEngine;
using System.Collections;

public class RequiredItemModule : InteractionModule {

	[SerializeField]
	InventoryObject keyItem;


	public override bool CheckInteract ()
	{
		
		return InventoryMan.main.selectedObject == keyItem;
	
	}


}
