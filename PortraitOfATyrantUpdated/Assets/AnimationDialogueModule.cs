using UnityEngine;
using System.Collections;

public class AnimationDialogueModule : DialogueNode {
	
	[SerializeField]
	private string animationToPlay;
	
	[SerializeField]
	Animator animator;
	
	public override void Init ()
	{
		
		base.Init ();
		if (animationToPlay != ""){
			if (animator == null) animator = gameObject.ForceGetComponent<Animator>();
			animator.Play(animationToPlay, 0, 0);
		}
	}
	
	
}