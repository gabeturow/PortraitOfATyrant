using UnityEngine;
using System.Collections;

public class PointsOnEnter : OnEnterNode {
		
		public int howManyPoints;
		public string conditionNameToSet;
		public bool conditionStateToSet=true;

	public override void OnEnter (DialogueNode node){
		{

			PointsController.main.points += howManyPoints;
			GameMan.main.conditionals.SetValue(conditionNameToSet, conditionStateToSet);

		}

	}
}