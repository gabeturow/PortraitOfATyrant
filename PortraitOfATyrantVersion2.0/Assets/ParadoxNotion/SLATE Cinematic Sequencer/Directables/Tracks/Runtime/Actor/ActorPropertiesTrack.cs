﻿namespace Slate{

	[Attachable(typeof(ActorGroup))]
	public class ActorPropertiesTrack : PropertiesTrack {

		//just add some defaults for convenience
		protected override void OnCreate(){
			base.OnCreate();
			animationData.TryAddParameter( typeof(UnityEngine.Transform).RTGetProperty("localPosition"), actor, null );
			animationData.TryAddParameter( typeof(UnityEngine.Transform).RTGetProperty("localEulerAngles"), actor, null );
		}
	}
}