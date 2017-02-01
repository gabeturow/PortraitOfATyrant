using UnityEngine;
using System.Collections;

public class turnOnturnOff : MonoBehaviour {

	public GameObject objectToTurnOff;

	// Use this for initialization
	IEnumerator Start () {
		yield return StartCoroutine(GoGo());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator GoGo(){
		yield return new WaitForSeconds(0f);
		if(objectToTurnOff!=null){
			objectToTurnOff.GetComponent<CanvasGroupFader>().displaying=false;
		}
	}
}
