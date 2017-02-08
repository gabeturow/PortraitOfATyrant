using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InventoryMan : MonoBehaviour {

	public static InventoryMan main;


	public InventoryObject selectedObject{get; set;}
	public InventoryObject blank{get; set;}

	private List<InventoryObject> _items;
	protected List<InventoryObject> items{
		get{ 
			if (_items == null) _items = new List<InventoryObject>();
			return _items; 
		}
	}

	private List<InventoryObjectRenderer> itemRenderers = new List<InventoryObjectRenderer>();

	public static InventoryFlyoutMenu flyoutMenu;

	void Awake(){
		main = this;
		flyoutMenu = PrefabManager.Instantiate("InventoryFlyout").GetComponent<InventoryFlyoutMenu>();
		flyoutMenu.transform.SetParent(this.transform);
		flyoutMenu.Hide();
	}

	void Update(){
		for(int i = 0; i < itemRenderers.Count; i++){
			itemRenderers[i].index = i;
			itemRenderers[i].UpdateMe();
		}
	}

	public void ShowFlyoutMenu(InventoryObjectRenderer objRenderer){
		flyoutMenu.Show(objRenderer);
	}

	public void HideFlyoutMenu(InventoryObjectRenderer objRenderer){
		flyoutMenu.Hide();
	}

	public void SelectObject(InventoryObjectRenderer objrenderer){
		this.selectedObject = objrenderer.inventoryObj;

	}

	public InventoryObject[] GetGrievances(){
		return items.Where(
			i=>{return i.type == InventoryObject.ObjectType.Grievance;}
		).ToArray();
	}

	public void Add(InventoryObject obj){
		items.Add(obj);

		if (obj.type != InventoryObject.ObjectType.Grievance){
			//add as sprite to inventory bar
			var rend = InventoryObjectRenderer.NewRenderer(obj);
			rend.transform.parent = this.transform;
			itemRenderers.Add(rend);
		}
		Destroy(flyoutMenu.gameObject);
		flyoutMenu = PrefabManager.Instantiate("InventoryFlyout").GetComponent<InventoryFlyoutMenu>();
		flyoutMenu.transform.SetParent(this.transform);
	}

	public void Remove(InventoryObject obj){
		items.Remove(obj);
	}

	public void RemoveRender(string ItemName){
		for(int x=0; x<itemRenderers.Count; x++){
			if(itemRenderers[x].name!=null && itemRenderers[x].name==ItemName){
					itemRenderers.RemoveAt(x);
					items.RemoveAt(x);
					_items.RemoveAt(x);
				this.transform.FindDeepChild(ItemName).gameObject.SetActive(false);
			}
		}
	}

	public bool HasItem(InventoryObject obj){
		for(int i = 0; i < items.Count; i++){
			if (items[i].name == obj.name && items[i].description == obj.description){
				return true;
			}
		}
		return false;
	}


}
