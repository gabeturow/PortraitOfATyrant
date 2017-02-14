using UnityEngine;
using System.Collections;

public class AnimationDialogueOnEnter :OnEnterNode {
	
		[SerializeField]
		public string animationToPlay;

		[SerializeField]
		public Animator animator;

		public override void OnEnter (DialogueNode node){
			if (animationToPlay != ""){
				if (animator == null) animator = gameObject.ForceGetComponent<Animator>();
				animator.Play(animationToPlay, 0, 0);
			}
		}


	}
