using UnityEngine;
using System.Collections;

public class SetConditionModule : InteractionModule {

	[SerializeField]
	string conditional;
	[SerializeField]
	bool valueToSet;
	[SerializeField]
	float delayTime=0;


	public override void OnInteract ()
	{
		base.OnInteract ();
		//GameMan.main.controller.ChangeTheMusic();
		Invoke("GoSetValue", delayTime);
	}

	void GoSetValue(){
		GameMan.main.conditionals.SetValue(conditional, valueToSet);
	}
}
