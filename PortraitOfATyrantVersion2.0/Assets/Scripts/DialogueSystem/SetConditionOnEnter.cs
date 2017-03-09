using UnityEngine;
using System.Collections;

public class SetConditionOnEnter : OnEnterNode {

	[SerializeField]
	public string conditional;
	[SerializeField]
	public bool valueToSet;

	public override void OnEnter (DialogueNode node){
		//GameMan.main.controller.ChangeTheMusic();
		GameMan.main.conditionals.SetValue(conditional, valueToSet);
	}
}

