using UnityEngine;
using System.Collections;

public class EnableGameObjectModule : InteractionModule {

	public bool valueToSet;
	public float delayTurnOn;
	public Transform transform;


	IEnumerator GoTransform(){
		yield return new WaitForSeconds(delayTurnOn);
		if(transform!=null){
			transform.gameObject.SetActive(valueToSet);
		}
	}


	public override void OnInteract ()
	{
		base.OnInteract ();

	StartCoroutine(GoTransform());
		
	}

}

