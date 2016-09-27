using UnityEngine;
using System.Collections;

public class AddPointsInteractionModule : InteractionModule {
	
public int howManyPoints;
	//public bool givenPointsAlready;
	public string conditionName;
	//public PointsController pointman;
	//public 
	public override void OnInteract ()
	{
//		pointman=GetComponent(PointsController);
		PointsController.main.points += howManyPoints;
		GameMan.main.conditionals.SetValue(conditionName, true);
	//	GameMan.main.conditionals.SetValue("GIVENPOINTSALREADY", true);
	//	}
	}
	
}
