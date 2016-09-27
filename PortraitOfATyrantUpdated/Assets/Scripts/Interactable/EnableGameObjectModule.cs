using UnityEngine;
using System.Collections;

public class EnableGameObjectModule : InteractionModule {

	public bool valueToSet;
//	public GameObject objectToSet;
	public Transform transform;

	public override void OnInteract ()
	{
		base.OnInteract ();
		//objectToSet.SetActive(valueToSet);
		if(transform!=null){
			transform.gameObject.SetActive(valueToSet);
		}
	}

}

