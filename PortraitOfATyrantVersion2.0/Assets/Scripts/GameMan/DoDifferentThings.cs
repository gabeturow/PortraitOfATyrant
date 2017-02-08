using UnityEngine;
using System.Collections;

public class DoDifferentThings : MonoBehaviour {

	public GameObject toTurnOn;
	public GameObject[] toTurnOn1;
	public GameObject slideshowFader;

	// Use this for initialization

	public void TurnOnGameObject(){

		for(int x=0; x<toTurnOn1.Length;x++){
		toTurnOn1[x].SetActive(true);
		}

				if(GameMan.main.conditionals.GetValue("STARTED")){
					
					slideshowFader.GetComponent<CanvasGroupFader>().displaying=false;
				}else{
		GameMan.main.conditionals.SetValue("STARTED", true);		
		toTurnOn.SetActive(true);
				}
	}


}
