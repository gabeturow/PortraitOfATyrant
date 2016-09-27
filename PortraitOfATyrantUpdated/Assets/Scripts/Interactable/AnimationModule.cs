using UnityEngine;
using System.Collections;

public class AnimationModule : InteractionModule {

	[SerializeField]
	private string animationToPlay;

	[SerializeField]
	Animator animator;

	public override void OnInteract ()
	{
		if (animationToPlay != ""){
			if (animator == null) animator = gameObject.ForceGetComponent<Animator>();
			animator.Play(animationToPlay, 0, 0);
		}
	}


}