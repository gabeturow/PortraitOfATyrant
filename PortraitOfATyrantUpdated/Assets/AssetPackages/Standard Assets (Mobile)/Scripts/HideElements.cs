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

		this.gameObject.SetActive(false);
		GameMan.main.conditionals.SetValue("GRIEVANCE", false);
	}
}
