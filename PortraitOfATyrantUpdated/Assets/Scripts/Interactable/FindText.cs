using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FindText : MonoBehaviour {
	GrievanceButton grievanceButton;
	public string textToFind;

	// Use this for initialization
	void Awake() {
		grievanceButton=GetComponentInChildren<GrievanceButton>();

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Text>().text=textToFind;
	}
}
