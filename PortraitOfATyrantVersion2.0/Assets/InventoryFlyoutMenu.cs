using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryFlyoutMenu : MonoBehaviour {

	public static InventoryFlyoutMenu main;

	[SerializeField]
	Button useButton;

	[SerializeField]
	Button inspectButton;


	InventoryObjectRenderer currentItem;
	CanvasGroupFader currentFader;
	public int flyoutPosition=0;

	public RectTransform rectTransform {get; private set;}

	void Awake(){
		rectTransform = GetComponent<RectTransform>();
		useButton.onClick.AddListener(Use);
		inspectButton.onClick.AddListener(Inspect);
		currentFader=GetComponent<CanvasGroupFader>();

	}

	public void Show(InventoryObjectRenderer inventoryItem){
		rectTransform.position = new Vector3(inventoryItem.transform.position.x-flyoutPosition, inventoryItem.transform.position.y, inventoryItem.transform.position.z);
		currentItem = inventoryItem;
		this.gameObject.SetActive(true);
		currentFader.displaying=true;
		currentFader.SetSpeed(20f);

		InventoryMan.main.SelectObject(currentItem);
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
		currentFader.displaying=false;
		//this.gameObject.SetActive(false);
	}

}
