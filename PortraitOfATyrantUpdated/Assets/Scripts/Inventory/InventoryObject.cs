using UnityEngine;
using System.Collections;

public class InventoryObject : ScriptableObject {

	public enum ObjectType{
		Normal,
		Grievance
	}

	public string name;

	[TextArea(2, 4)]
	public string description;
	public Sprite inventorySprite;

	public ObjectType type;


}



