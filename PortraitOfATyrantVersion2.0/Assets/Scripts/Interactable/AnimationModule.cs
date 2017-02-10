﻿using UnityEngine;
using System.Collections;

public class AnimationModule : InteractionModule {

	[SerializeField]
	private string animationToPlay;
	[SerializeField]
	private float delayTime=0;
	[SerializeField]
	public string whichTrigger="";
	[SerializeField]
	public bool turnLeft;


	[SerializeField]
	Animator animator;

	void PlayAnimation(){
		animator.Play(animationToPlay, 0, 0);
		if(whichTrigger!=""){
			animator.SetTrigger(whichTrigger);
		}
	}
		
	public override void OnInteract ()
	{
		if (animationToPlay != ""){
			if (animator == null) animator = gameObject.ForceGetComponent<Animator>();
			Invoke("PlayAnimation", delayTime);


		}
		//TurnLeft
		CharacterAnimation characterNew;
		if(turnLeft){
			characterNew = DialogueViewer.main.characterOne.GetComponent<CharacterAnimation>();
			characterNew.TurnCharacterLeft(true);
		}
	}


}