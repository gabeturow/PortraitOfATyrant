using UnityEngine;
using System.Collections;

public class ToggleConditionModule : InteractionModule {
	
	[SerializeField]
	string conditional;
	[SerializeField]
	bool valueToSet;
	
	public override void OnInteract ()
	{
		base.OnInteract ();
		valueToSet=!valueToSet;
		GameMan.main.conditionals.SetValue(conditional, valueToSet);
	}
	
}

