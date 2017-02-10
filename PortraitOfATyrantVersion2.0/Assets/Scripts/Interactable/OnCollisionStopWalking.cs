using UnityEngine;
using System.Collections;

public class OnCollisionStopWalking : MonoBehaviour {

	// Use this for initialization


	void OnTriggerEnter2D(Collider2D collider2d) {

		float currentTarget=0;
		var animEnum = System.Enum.Parse(typeof(CharacterAnimation.PlayingAnim), "Idle", true);
		GameMan.main.character.render.anim.action = (CharacterAnimation.PlayingAnim)animEnum;
		Debug.Log("hitSomething");
		if(GameMan.main.character.GetComponentInChildren<CharacterAnimation>().Side.transform.localScale.x>0){
			currentTarget=GameMan.main.character.transform.localPosition.x +.5f;
		}else{
			currentTarget=GameMan.main.character.transform.localPosition.x -.5f;	
		}

		GameMan.main.character.GetComponentInChildren<CharacterMotor>().target.x=currentTarget;



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
