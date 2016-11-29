using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryFlyoutMenu : MonoBehaviour {

	[SerializeField]
	Button useButton;

	[SerializeField]
	Button inspectButton;

	InventoryObjectRenderer currentItem;

	public RectTransform rectTransform {get; private set;}

	void Awake(){
		rectTransform = GetComponent<RectTransform>();
		useButton.onClick.AddListener(Use);
		inspectButton.onClick.AddListener(Inspect);
	}

	public void Show(InventoryObjectRenderer inventoryItem){
		rectTransform.position = inventoryItem.transform.position;
		currentItem = inventoryItem;
		this.gameObject.SetActive(true);
	}

	void Use(){
		if (currentItem != null) {
			InventoryMan.main.SelectObject(currentItem);
		}
		Hide();
	}

	void Inspect(){
		if (currentItem != null) {
			UIMan.itemInspector.Show(this.currentItem, null);
		}
		Hide();
	}

	public void Hide(){
		this.gameObject.SetActive(false);
	}

}
