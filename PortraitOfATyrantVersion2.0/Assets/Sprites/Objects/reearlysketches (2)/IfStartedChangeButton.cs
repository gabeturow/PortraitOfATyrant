using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IfStartedChangeButton : MonoBehaviour {

	public string changeButtonTo;
	public GameObject skipButton;

	void Start(){


	}
	void Update () {
		if(GameMan.main.conditionals.GetValue("STARTED")){
			this.GetComponentInChildren<Text>().text=""+changeButtonTo;
	}
	}

	public void ShowSkipButton(){
		if(GameMan.main.conditionals.GetValue("CUTSCENERUNNING")){
				skipButton.SetActive(true);
			}
	}
	
}
