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

		try{
			//parse the string into the animation enum
			var animEnum = System.Enum.Parse(typeof(CharacterAnimation.PlayingAnim), animationToPlay, true);
			//play the animation
			GameMan.main.character.render.anim.action = (CharacterAnimation.PlayingAnim)animEnum;

		}
		catch(System.ArgumentException e){
			//failed to parse the string
			Debug.LogError(e);
		}
		}


	}
