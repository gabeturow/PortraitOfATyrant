using UnityEngine;
using System.Collections;

public class ConditionalModule : InteractionModule {

	public string conditionToCheck;
	public bool requiredCondition;

	public override bool CheckInteract ()
	{
		return (GameMan.main.conditionals.GetValue(conditionToCheck) == requiredCondition);
	}

}
