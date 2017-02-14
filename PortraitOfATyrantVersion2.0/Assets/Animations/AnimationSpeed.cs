using UnityEngine;
using System.Collections;

public class AnimationSpeed : MonoBehaviour {
	public enum ObjType{
		Character, Object}
	public ObjType type;
	public AnimationState currentState;
	public Animator animator;

	public CharacterAnimation characterAnimationScript;

	public float animationSpeed;

	// Use this for initialization
	void Start () {
		if (type == ObjType.Character){
			if (characterAnimationScript == null){
				Debug.Log("CHARACTER ANIMATION SCRIPT is not determined");
			}
		} else if (type == ObjType.Object){
			if (animator == null){
				Debug.Log("Object's ANIMATOR is not determined");
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (type == ObjType.Character){
			if(animator!=null){
			animator = characterAnimationScript.currentAnimator;
			}
		}
		if(animator!=null){
		animator.speed = animationSpeed;
		}
	}
}
