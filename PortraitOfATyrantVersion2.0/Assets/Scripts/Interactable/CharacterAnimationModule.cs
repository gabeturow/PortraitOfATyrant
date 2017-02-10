using UnityEngine;
using System.Collections;
using UnityEditor;

public class CharacterAnimationModule : InteractionModule {

	//this is a string because unity treats enums in the inspector as ints
	//it gives unintended behaviour when rearranging the enum delcarations.
	public string animationName;
	public string animationParameter;
	public bool parameterOn;
	public GameObject newGame;




	public override void OnInteract ()
	{

		Debug.Log(animationName);
		//try convert string to enum
		try{
			//parse the string into the animation enum
			var animEnum = System.Enum.Parse(typeof(CharacterAnimation.PlayingAnim), animationName, true);
			//play the animation
			GameMan.main.character.render.anim.action = (CharacterAnimation.PlayingAnim)animEnum;

		}
		catch(System.ArgumentException e){
			//failed to parse the string
			Debug.LogError(e);
		}

	}

}
