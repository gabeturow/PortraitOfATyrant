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
		scrollview.scrollSensitivity=2f;
		scrollview.verticalScrollbar.value=1;
		contentText = scrollview.content.GetComponentInChildren<Text>();
		this.itemImage = itemImg;
	}

	public override void Show(Sprite itemSprite, System.Action onFinishInspect)
	{
		scrollview.verticalScrollbar.value=1;
		base.Show(itemSprite, onFinishInspect);
		this.hideTimer = 0f;
	}

	public void Show(Sprite itemSprite, string text, System.Action onFinishInspect)
	{
		scrollview.verticalScrollbar.value=1;
		this.contentText.text = text;
		Show(itemSprite, onFinishInspect);
	}

	public void Show(InventoryObjectRenderer obj, System.Action onFinishInspect){
		scrollview.verticalScrollbar.value=1;
		var description = obj.inventoryObj.longDescription;
		Show(obj.inventoryObj.inventorySprite, description, onFinishInspect);
	}

}
