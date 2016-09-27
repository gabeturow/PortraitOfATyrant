using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GrievanceButton : Button {

	InventoryObject item;

	[SerializeField]
	Text childText;

	System.Action<InventoryObject> onSelect;

	protected override void Awake(){
		base.Awake();
		childText = GetComponentInChildren<Text>();
	}

	public void Init(InventoryObject item, System.Action<InventoryObject> onSelect){
		this.item = item;
		this.childText.text = 
			"<size=20>" + 
			item.name +
			"</size>\n" + 
			item.description;
		this.onSelect = onSelect;
	}

	public override void OnSelect (UnityEngine.EventSystems.BaseEventData eventData)
	{
		base.OnSelect (eventData);
		if (this.onSelect != null){
			this.onSelect(this.item);
		}
	}

}
