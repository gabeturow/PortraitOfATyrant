using UnityEngine;
using System.Collections;

public class DialogueConditional : DialogueNode {

	public DialogueNode trueNode;
	public DialogueNode falseNode;

	public string conditionToCheck;

	public override void Init ()
	{
		base.Init ();
		bool condition = GameMan.main.conditionals.GetValue(conditionToCheck);
		this.nextNode = condition ? trueNode : falseNode;
	}
/*
	protected override void DrawConnectors ()
	{
		if (trueNode != null){
			DrawArrow.Connector(transform.position, trueNode.transform.position - transform.position);
		}
		if (falseNode != null){
			DrawArrow.Connector(transform.position, falseNode.transform.position - transform.position);
		}
	}

*/

}
