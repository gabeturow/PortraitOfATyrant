using UnityEngine;
using System.Collections;

public class HasInventoryModule : InteractionModule {

	public InventoryObject item;

	public override bool CheckInteract ()
	{
		return InventoryMan.main.HasItem(item);
	}


}
