using UnityEngine;
using System.Collections;

public class ConditionalAnimate : MonoBehaviour {

		
	[SerializeField]
	private string animationToPlay;
	
	[SerializeField]
	Animator animator;

	[SerializeField]
	string BooleanName;
	
	void Start()
	{
		if (animationToPlay != "" && GameMan.main.conditionals.GetValue(BooleanName)){
			if (animator == null) animator = gameObject.ForceGetComponent<Animator>();
			animator.Play(animationToPlay, 0, 0);
		}
	}
	}
