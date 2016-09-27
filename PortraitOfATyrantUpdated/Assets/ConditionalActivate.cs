using UnityEngine;
using System.Collections;

public class ConditionalActivate : MonoBehaviour {

	public GameObject ObjectToActivate;
	public string BooleanName;
	public bool turnItOn=true;

	// Update is called once per frame
	void Update () {


		if(GameMan.main.conditionals.GetValue(BooleanName)){
			ObjectToActivate.SetActive(turnItOn);
		}

	}
}
