using UnityEngine;
using System.Collections;

public class DoDifferentThings : MonoBehaviour {

	public GameObject toTurnOn;
	public GameObject[] toTurnOn1;
	public GameObject slideshowFader;
	public string[] setTheseBoolNames;
	public bool[] setToTheseStates;

	// Use this for initialization

	public void TurnOnGameObject(){

		//Turn on the Conditionals Referenced in the array, setTheseBoolNames.

		for(int conditionals=0; conditionals<setTheseBoolNames.Length; conditionals++){
		GameMan.main.conditionals.SetValue(setTheseBoolNames[conditionals], setToTheseStates[conditionals]);
		}

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
