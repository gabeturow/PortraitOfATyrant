using UnityEngine;
using System.Collections;

public class DialogueGrievancePuzzle : DialogueNode {

	public DialogueNode succeedNode;
	public DialogueNode failNode;

	public GrievancePuzzle puzzle;

	public override void DoBlock ()
	{
		GrievanceSolver.main.StartPuzzle(puzzle, 
			(solved)=>{
				if (solved){
					nextNode = succeedNode;
				}
				else{
					nextNode = failNode;
				}
				GrievanceSolver.main.Hide();
				UnBlock();
			});
	}
	/*
	protected override void DrawConnectors ()
	{
		if (succeedNode != null){
			DrawArrow.Connector(transform.position, succeedNode.transform.position - transform.position);
		}
		if (failNode != null){
			DrawArrow.Connector(transform.position, failNode.transform.position - transform.position);
		}
	}

*/
}
