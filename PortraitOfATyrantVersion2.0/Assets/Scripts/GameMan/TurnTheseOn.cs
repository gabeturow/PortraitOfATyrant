using UnityEngine;
using System.Collections;

public class TurnTheseOn : MonoBehaviour {

	public GameObject[] turnTheseEnable;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableThese(){
		for(int x=0; x<turnTheseEnable.Length;x++){
			turnTheseEnable[x].GetComponent<CanvasGroupFader>().displaying=true;
		}
	}
}
