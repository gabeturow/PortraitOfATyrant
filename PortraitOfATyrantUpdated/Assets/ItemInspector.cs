using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInspector : PopUpViewer {

	[SerializeField]
	Image itemImg;

	ScrollRect scrollview;
	Text contentText;

	protected override void Awake(){
		base.Awake();
		scrollview = GetComponentInChildren<ScrollRect>();
		contentText = scrollview.content.GetComponentInChildren<Text>();
		this.itemImage = itemImg;
	}

	public override void Show(Sprite itemSprite, System.Action onFinishInspect)
	{
		base.Show(itemSprite, onFinishInspect);
		this.hideTimer = 0f;
	}

	public void Show(Sprite itemSprite, string text, System.Action onFinishInspect)
	{
		this.contentText.text = text;
		Show(itemSprite, onFinishInspect);
	}

	public void Show(InventoryObjectRenderer obj, System.Action onFinishInspect){
		var description = obj.inventoryObj.longDescription;
		Show(obj.inventoryObj.inventorySprite, description, onFinishInspect);
	}

}
