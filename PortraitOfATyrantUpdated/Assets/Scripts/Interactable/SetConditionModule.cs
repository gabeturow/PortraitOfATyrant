using UnityEngine;
using System.Collections;

public class SetConditionModule : InteractionModule {

	[SerializeField]
	string conditional;
	[SerializeField]
	bool valueToSet;

	public override void OnInteract ()
	{
		base.OnInteract ();
		GameMan.main.conditionals.SetValue(conditional, valueToSet);
	}

}
