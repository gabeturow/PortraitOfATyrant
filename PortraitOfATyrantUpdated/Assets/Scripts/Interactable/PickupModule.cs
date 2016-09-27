using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupModule : InteractionModule {

	[Tooltip("Make ID unique per each pickup item")]
	public string id = "";
	private string Identifier{
		get{ if (id == ""){
				return this.gameObject.name;
			}
			return id;
		}
	}

	private static HashSet<string> _pickups;
	private static HashSet<string> pickups{
		get { if (_pickups == null) _pickups = new HashSet<string>();
			return _pickups; 
		}
	}

	public InventoryObject inventoryObj;

	void Awake(){
		if (pickups.Contains(this.Identifier)){
			Destroy(this.gameObject);
		}
	}

	public override void OnInteract(){
		if (inventoryObj != null){
			InventoryMan.main.Add(inventoryObj);
			//pick up by adding this object to the internal hashset
			pickups.Add(this.Identifier);
		}
		Destroy(this.gameObject);
	}
}
