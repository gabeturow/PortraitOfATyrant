using UnityEngine;
using System.Collections;

public class RequiredPointsModule : InteractionModule {
		
	public int minimumRequired;
//	public PointsController pointman;
		
		public override bool CheckInteract ()
		{
		return (PointsController.main.points>=minimumRequired);
		}
		
	}
