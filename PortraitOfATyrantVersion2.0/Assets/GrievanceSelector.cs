using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Klak.Math;
using System.Linq;

public class GrievanceSelector : MonoBehaviour {
	public static GrievanceSelector main;

	CanvasGroupFader fader;

	GrievanceButton buttonTemplate;
	GrievanceButton[] buttonInstances = new GrievanceButton[0];


	public void ShowGrievances(System.Action<InventoryObject> onSelect, params InventoryObject[] items){
		//delete the old clones
		ClearButtons();
		buttonInstances = new GrievanceButton[items.Length];

		//clone the template to create the grievance list
		for(int i = 0; i < items.Length; i++){
			var newButton = (Instantiate(buttonTemplate.gameObject) as GameObject).GetComponent<GrievanceButton>();
			newButton.transform.parent = buttonTemplate.transform.parent;
			newButton.Init(items[i], onSelect);
			newButton.gameObject.SetActive(true);
			buttonInstances[i] = newButton;
		}
		fader.displaying = true;
	}

	public void Hide(){
		fader.displaying = false;
	}

	void ClearButtons(){
		for(int i = 0; i < buttonInstances.Length; i++){
			Destroy(buttonInstances[i].gameObject);
		}
		buttonInstances = new GrievanceButton[0];
	}


	void Awake(){
		main = this;
		buttonTemplate = GetComponentInChildren<GrievanceButton>();
		buttonTemplate.gameObject.SetActive(false);
		fader = gameObject.ForceGetComponent<CanvasGroupFader>();
	}


}
