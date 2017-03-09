using UnityEngine;
using System.Collections;

public class TurnTheseOn : MonoBehaviour {

	public GameObject[] turnTheseEnable;
	public GameObject[] turnTheseOff;
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

		for(int y=0; y<turnTheseOff.Length;y++){
			turnTheseOff[y].GetComponent<CanvasGroupFader>().displaying=false;
		}
	}
}
