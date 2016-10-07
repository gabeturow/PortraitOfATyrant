using UnityEngine;
using System.Collections;

public class HideElements : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HideAll(){
		GameMan.main.conditionals.SetValue("CONFLICT", false);
		GameMan.main.conditionals.SetValue("BELOWDECK", true);
		this.GetComponent<CanvasGroupFader>().displaying=false;
		//this.gameObject.SetActive(false);

	}
}
