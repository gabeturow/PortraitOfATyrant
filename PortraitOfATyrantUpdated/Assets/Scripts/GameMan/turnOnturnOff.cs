using UnityEngine;
using System.Collections;

public class turnOnturnOff : MonoBehaviour {

	public GameObject objectToTurnOff;

	// Use this for initialization
	void Start () {
		if(objectToTurnOff!=null){
		objectToTurnOff.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
