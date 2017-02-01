using UnityEngine;
using System.Collections;

public class TurnMenuBackOn : MonoBehaviour {

	public GameObject slideshowNode;
	// Use this for initialization
	void Start () {
		slideshowNode.GetComponent<DisplayIt>().DisplayPanel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
