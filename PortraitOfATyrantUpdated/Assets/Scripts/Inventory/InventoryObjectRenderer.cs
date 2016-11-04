using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryObjectRenderer : Selectable {

	const float ITEM_LEFT_SPACE = 50;
	const float ITEM_SPACING = 70;

	public int index;
	ManualLocalRectPositionSpring posSpring;
	ManualLocalScaleSpring scaleSpring;
	public Image spriteRend{get; private set;}
	public InventoryObject inventoryObj{get; set;}


	public static InventoryObjectRenderer NewRenderer(InventoryObject o){
		GameObject i = new GameObject(o.name);
		var rend = i.AddComponent<InventoryObjectRenderer>();
		rend.inventoryObj = o;
		rend.spriteRend.sprite = o.inventorySprite;
		rend.spriteRend.rectTransform.anchorMin = rend.spriteRend.rectTransform.anchorMax = new Vector2(0f, 0.5f);
		rend.spriteRend.rectTransform.sizeDelta = new Vector2(50f, 50f);
		return rend;
	}


	public override void OnSelect (UnityEngine.EventSystems.BaseEventData eventData)
	{
		base.OnSelect (eventData);
		scaleSpring.velocity -= new Vector3(5f, 5f, 5f);
		scaleSpring.dampingRatio = .7f;
		InventoryMan.main.selectedObject = this.inventoryObj;
	}


	void Awake(){
		posSpring = gameObject.ForceGetComponent<ManualLocalRectPositionSpring>();
		scaleSpring = gameObject.ForceGetComponent<ManualLocalScaleSpring>();
		spriteRend = gameObject.ForceGetComponent<Image>();
	}

	public void UpdateMe(){
		float yTarget = 0f;
		if (this.currentSelectionState == SelectionState.Highlighted){
			yTarget = Mathf.Sin(Time.time * 4f) * 20f + 10f;
		}
		posSpring.target = new Vector2(yTarget,index * ITEM_SPACING + ITEM_LEFT_SPACE);
		posSpring.UpdateMe();
		scaleSpring.UpdateMe();
	}


}
