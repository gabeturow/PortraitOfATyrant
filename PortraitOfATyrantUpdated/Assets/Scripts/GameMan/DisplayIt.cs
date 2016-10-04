using UnityEngine;
using System.Collections;

public class DisplayIt : MonoBehaviour {

	public bool displayGo;
	CanvasGroupFader canvas;
	// Use this for initialization
	void Start () {
		canvas=GetComponent<CanvasGroupFader>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayPanel(){
		canvas.displaying=displayGo;
	}
}
