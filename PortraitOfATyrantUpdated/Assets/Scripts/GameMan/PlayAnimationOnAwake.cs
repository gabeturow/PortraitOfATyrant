using UnityEngine;
using System.Collections;

public class PlayAnimationOnAwake: InteractionModule {

	[SerializeField]
	private string animationToPlay;

	[SerializeField]
	Animator animator;

	public string BooleanName;
	public bool stateToCheckAgainst;


	void Start () {
		if(GameMan.main.conditionals.GetValue(BooleanName)==stateToCheckAgainst){
			if (animationToPlay != ""){
				if (animator == null) animator = gameObject.ForceGetComponent<Animator>();
				animator.Play(animationToPlay, 0, 0);
				}
			}

		}
	}