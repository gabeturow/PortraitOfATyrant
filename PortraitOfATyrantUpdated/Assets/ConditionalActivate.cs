using UnityEngine;
using System.Collections;

public class ConditionalActivate : MonoBehaviour {

	public GameObject ObjectToActivate;
	public Animator AnimationObjectToActivate;
	public string BooleanName;
	public bool turnItOn=true;

	// Update is called once per frame
	void Start () {


		if(GameMan.main.conditionals.GetValue(BooleanName)){
			if(ObjectToActivate!=null){
			ObjectToActivate.SetActive(turnItOn);
			}
			if(AnimationObjectToActivate!=null){
				AnimationObjectToActivate.enabled=turnItOn;
			}
		}

	}
}
