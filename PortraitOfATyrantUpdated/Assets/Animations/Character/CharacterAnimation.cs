﻿using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
	public GameObject Front, Side, Back;
	public Animator currentAnimator;

	public enum Face{
		Front, Left, Right, Back}
	public Face facing;

	public enum PlayingAnim{
		Idle, Walk, Talk, Laugh, PickUp_Hi, PickUp_Med, PickUp_Lo, Inventory, InventoryUse,
		ClimbUp, PlayCards, TightRope, Captain, LightLamp, TurnLeft}
	public PlayingAnim action;

	public bool stairUp, stairDown;

	void Start () {

	}

	void Update () {

		//CHANGE SPRITE OBJECT DEPENDING ON WHICH WAY HE IS FACING

		switch (facing){
		//IF FACING THE FRONT
		case Face.Front:
			Front.SetActive(true);
			Back.SetActive(false);
			Side.SetActive(false);

			currentAnimator = Front.GetComponent<Animator>();
			break;

			//IF FACING LEFT
		case Face.Left:
			Front.SetActive(false);
			Back.SetActive(false);
			Side.SetActive(true);
			Side.transform.localScale = new Vector3(1, 1, 1);
		//	Back.transform.localScale = new Vector3(0, 0, 0);

			currentAnimator = Side.GetComponent<Animator>();
			break;

			//IF FACING RIGHT
		case Face.Right:
			Front.SetActive(false);
			Back.SetActive(false);
			Side.SetActive(true);
			Side.transform.localScale = new Vector3(-1, 1, 1);
		//	Back.transform.localScale = new Vector3(0, 0, 0);
			
			currentAnimator = Side.GetComponent<Animator>();
			break;

		case Face.Back:
			Front.SetActive(false);
		//	Side.SetActive(false);
			Back.SetActive(true);
		//	Side.transform.localScale = new Vector3(0, 0, 0);
			currentAnimator = Back.GetComponent<Animator>();
			break;
		}

		PlayAnim(action);

	}

	IEnumerator StopAnim(){
		Side.transform.localScale = new Vector3(0, 0, 0);
		currentAnimator.SetBool("ClimbUp", true);
		yield return new WaitForSeconds(7);
		facing=Face.Right;
		action = PlayingAnim.Walk;
		currentAnimator.SetBool("ClimbUp", false);

	Back.transform.localScale = new Vector3(0, 0, 0);
	
	Side.transform.localScale = new Vector3(1, 1, 1);
	
	}

	void PlayAnim(PlayingAnim anim){
		switch (anim){
		case PlayingAnim.Idle:
			currentAnimator.SetBool("Walking", false);

			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
				currentAnimator.SetBool("TurnLeft", false);
			} 
			break;
		case PlayingAnim.Talk:
			currentAnimator.SetBool("Walking", false);
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", true);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
			}
			break;
		case PlayingAnim.Walk:
			currentAnimator.SetBool("Walking", true);
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
			}
			if (!stairUp && stairDown){
				currentAnimator.SetTrigger("StairDown");
				stairDown = false;
			} else if (stairUp && !stairDown){
				currentAnimator.SetTrigger("StairUp");
				stairUp = false;
			}
			break;
		case PlayingAnim.Laugh:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", true);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
			}
			break;
		case PlayingAnim.TurnLeft:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("TurnLeft", true);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
			}
			break;
		case PlayingAnim.PickUp_Hi:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 1);
				currentAnimator.SetBool("TightRope", false);
				action = PlayingAnim.Idle;
			}
			break;
		case PlayingAnim.PickUp_Lo:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 3);
				currentAnimator.SetBool("TightRope", false);
				action = PlayingAnim.Idle;
			}
			break;
		case PlayingAnim.PickUp_Med:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 2);
				currentAnimator.SetBool("TightRope", false);
				action = PlayingAnim.Idle;
			}
			break;
		case PlayingAnim.Inventory:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetTrigger("Inventory");
				currentAnimator.SetBool("TightRope", false);
				action = PlayingAnim.Idle;
			}
			break;
		case PlayingAnim.InventoryUse:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetTrigger("Use");
				currentAnimator.SetBool("TightRope", false);
				action = PlayingAnim.Idle;
			}
			break;
		case PlayingAnim.ClimbUp:
			if(facing == Face.Left || facing == Face.Right ){
				Back.transform.localScale = new Vector3(1, 1, 1);
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
				currentAnimator.SetBool("TurnLeft", false);
				facing=Face.Back;
				StartCoroutine(StopAnim());
			} else if (facing == Face.Back){
				currentAnimator.SetBool("ClimbUp", true);
				currentAnimator.SetBool("PlayCards", true);
			}
			

			break;
		case PlayingAnim.PlayCards:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
			} else if (facing == Face.Back){
				currentAnimator.SetBool("ClimbUp", false);
				currentAnimator.SetBool("PlayCards", true);
			}
			break;
		case PlayingAnim.TightRope:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", true);
			} else if (facing == Face.Back){
				currentAnimator.SetBool("ClimbUp", false);
				currentAnimator.SetBool("PlayCards", false);
			}
			break;
		case PlayingAnim.Captain:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
				currentAnimator.SetBool("Captain", true);
			} else if (facing == Face.Back){
				currentAnimator.SetBool("ClimbUp", false);
				currentAnimator.SetBool("PlayCards", false);
			}
			break;
		case PlayingAnim.LightLamp:
			if (facing == Face.Left || facing == Face.Right){
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
				currentAnimator.SetTrigger("LightLamp");
				action = PlayingAnim.Idle;
			}
			break;
		}
	}
}
