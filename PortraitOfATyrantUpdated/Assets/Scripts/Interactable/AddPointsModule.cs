using UnityEngine;
using System.Collections;


public class AddPointsModule : DialogueNode {

	public int howManyPoints;
	public int resistancePointsChange;
	public string conditionName;
	//public PointsController pointsController;

	public override void Init ()
	{

		base.Init ();
		PointsController.main.points +=howManyPoints;
		PointsController.resistancePoints +=resistancePointsChange;

		GameMan.main.conditionals.SetValue(conditionName, true);
	}
	
	
}
