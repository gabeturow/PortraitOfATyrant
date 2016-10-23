using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IfStartedChangeButton : MonoBehaviour {

	void Start(){


	}
	void Update () {
		if(GameMan.main.conditionals.GetValue("STARTED")){
			this.GetComponentInChildren<Text>().text="Resume";
	}
}
}
