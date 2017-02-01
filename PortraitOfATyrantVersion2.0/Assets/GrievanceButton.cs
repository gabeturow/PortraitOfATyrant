using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class GrievanceButton : Button {

	public InventoryObject item;
	public Text findText;
	[SerializeField]
	Text childText;
	[SerializeField]


	System.Action<InventoryObject> onSelect;

	protected override void Awake(){
		base.Awake();
		childText = GetComponentInChildren<Text>();
		findText=GetComponentInParent<Text>();
//		replaceText=GetComponentInParent<ReplaceThisText>();
//		iconTemp=GetComponentInChildren<Image>();
	}

	public void Init(InventoryObject item, System.Action<InventoryObject> onSelect){
		this.item = item;
		this.childText.text = 
			"" + 
			item.name;
		
		//	item.description;
//		replaceText.flavorText=item.description;
		Debug.Log(this.item.description);

		gameObject.GetComponentInChildren<Image>().sprite=item.inventorySprite;
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
