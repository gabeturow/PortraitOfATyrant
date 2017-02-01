using UnityEngine;
using System.Collections;

public class DoDifferentThings : MonoBehaviour {

	public GameObject toTurnOn;
	public GameObject slideshowFader;
	// Use this for initialization

	public void TurnOnGameObject(){
				if(GameMan.main.conditionals.GetValue("STARTED")){
					slideshowFader.GetComponent<CanvasGroupFader>().displaying=false;
				}else{
		GameMan.main.conditionals.SetValue("STARTED", true);		
		toTurnOn.SetActive(true);
				}
	}


}
