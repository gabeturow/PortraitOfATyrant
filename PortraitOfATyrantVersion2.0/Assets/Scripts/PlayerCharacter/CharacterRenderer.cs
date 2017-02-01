using UnityEngine;
using System.Collections;
using Klak.Math;

public class CharacterRenderer : MonoBehaviour {


	public CharacterAnimation anim{get; private set;}

	void Awake(){
		anim = GetComponentInChildren<CharacterAnimation>();
	}

	public void SetVelocity(Vector2 velocity){
		if (Mathf.Abs(velocity.x) > .2f){
			anim.action = CharacterAnimation.PlayingAnim.Walk;
		}
		else if (anim.action == CharacterAnimation.PlayingAnim.Walk) {
			anim.action = CharacterAnimation.PlayingAnim.Idle;
		}

		if (velocity.x > .01f){
			anim.facing = CharacterAnimation.Face.Right;
		}
		else if (velocity.x < -.01f){
			anim.facing = CharacterAnimation.Face.Left;
		}

	}





//	public ManualLocalScaleSpring scaleSpring{get; private set;}
//
//	void Awake(){
//		scaleSpring = gameObject.GetComponent<ManualLocalScaleSpring>();
//	}
//
//	public void SetVelocity(Vector2 velocity){
//		if (velocity.x > .1f){
//			scaleSpring.target = new Vector3(1,1,1);
//		}
//		if (velocity.x < -.1f){
//			scaleSpring.target = new Vector3(-1,1,1);
//		}
//	}
//
//	// Update is called once per frame
//	void Update () {
//		scaleSpring.UpdateMe();
//	}
}
