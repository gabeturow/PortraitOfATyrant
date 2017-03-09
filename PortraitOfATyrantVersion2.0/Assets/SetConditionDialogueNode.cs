using UnityEngine;
using System.Collections;

public class SetConditionDialogueNode : DialogueNode {

	public string condition;
	public bool valueToSet = true;

	public override void Init ()
	{
		base.Init ();
//		GameMan.main.controller.ChangeTheMusic();
		GameMan.main.conditionals.SetValue(condition, valueToSet);
	}


}
