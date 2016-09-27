using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GrievanceSlot : Button {

	public InventoryObject slotted{get; private set;}

	public override void OnSelect (UnityEngine.EventSystems.BaseEventData eventData)
	{
		base.OnSelect (eventData);
		GrievanceSelector.main.ShowGrievances(SetSlot, InventoryMan.main.GetGrievances());
	}

	public void ClearSlot(){
		slotted = null;
		this.image.color = Color.white;
	}

	public void SetSlot(InventoryObject item){
		this.slotted = item;
		this.image.color = Color.red;
		GrievanceSelector.main.Hide();
	}

}
