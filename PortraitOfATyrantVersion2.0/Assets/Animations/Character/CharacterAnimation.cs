using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
	public static CharacterAnimation main;
	public GameObject Front, Side, Back;
	public Animator currentAnimator;
	public CanvasGroupFader openingCredits;
	public GameObject playerObject;
	public bool tempFirst=false;
	public bool introLevel1=false;

	public enum Face{
		Front, Left, Right, Back}
	public Face facing;

	public enum PlayingAnim{
		Idle, Walk, Talk, Laugh, PickUp_Hi, PickUp_Med, PickUp_Lo, Inventory, InventoryUse,
		ClimbUp, PlayCards, TightRope, Captain, LightLamp, TurnLeft, TurnAway, Crouch, DropCeiling, DropBegin}
	public PlayingAnim action;

	public bool stairUp, stairDown;



	void Start () {
		
	}
	public void TurnCharacterLeft(bool doIt){
		if(doIt){
			facing=Face.Left;
		}else{
			facing=Face.Right;
		}
	}
	void OpeningCredits(){
		openingCredits.displaying=false;
	}


	void TurnOnPlayer(){
		playerObject.transform.localScale= new Vector3(1.1f,1.1f,1.1f);
		TapInteractor.main.enabled = true;
	}

	void Update () {
	/*	Debug.Log(GameMan.main.conditionals.GetValue("STARTGAME"));
		if(GameMan.main.conditionals.GetValue("STARTED") && !tempFirst){
			//playerObject.transform.localScale=Vector3.zero;
			//TapInteractor.main.enabled = false;
			//playerObject.transform.localPosition=new Vector3(-15f,0f,0f);
			//Invoke("TurnOnPlayerLeft",7);
			Invoke("OpeningCredits",4);
			tempFirst=true;
		}
*/

		//CHANGE SPRITE OBJECT DEPENDING ON WHICH WAY HE IS FACING

		if(introLevel1){
			facing=Face.Front;
			currentAnimator.SetBool("DropCeiling", true);
		}
	
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
			Side.SetActive(false);
			Back.SetActive(true);
			//Side.transform.localScale = new Vector3(1, 1, 1);
			//Side.transform.localScale = new Vector3(0, 0, 0);
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
		currentAnimator.SetBool("Idle", true);
	//Front.transform.localScale = new Vector3(0, 0, 0);
	
	Side.transform.localScale = new Vector3(1, 1, 1);
	
	}

	IEnumerator StopDrop(){
		Side.SetActive(false);
		Side.transform.localScale = new Vector3(0, 0, 0);
		currentAnimator.SetBool("DropCeiling", true);
		yield return new WaitForSeconds(6);
		facing=Face.Right;
		action = PlayingAnim.Idle;
		currentAnimator.SetBool("DropCeiling", false);
		currentAnimator.SetBool("Idle", true);
		Front.transform.localScale = new Vector3(0, 0, 0);

		Side.transform.localScale = new Vector3(1, 1, 1);
		Side.SetActive(true);
	}

	IEnumerator TurnBack(){
		
		currentAnimator.SetBool("Idle", true);
		yield return new WaitForSeconds(.1f);
		//Side.transform.localScale = new Vector3(0, 0, 0);
		facing=Face.Back;

		currentAnimator.SetBool("TurnAway", true);
		yield return new WaitForSeconds(4.5f);
	//	facing=Face.Left;
		action = PlayingAnim.Walk;
		currentAnimator.SetBool("TurnAway", false);

		//Back.transform.localScale = new Vector3(0, 0, 0);

		//Side.transform.localScale = new Vector3(1, 1, 1);

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
		
		case PlayingAnim.DropCeiling:
			if(facing == Face.Left || facing == Face.Right ){
				Back.transform.localScale = new Vector3(1, 1, 1);
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
				currentAnimator.SetBool("TurnLeft", false);
				facing=Face.Front;
				StartCoroutine(StopDrop());
			} else if (facing == Face.Front){
				currentAnimator.SetBool("DropCeiling", true);

			}
			break;

		case PlayingAnim.DropBegin:
			if(facing == Face.Left || facing == Face.Right ){
				Back.transform.localScale = new Vector3(1, 1, 1);
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
				currentAnimator.SetBool("TurnLeft", false);
				facing=Face.Front;
				//StartCoroutine(StopDrop());
			} else if (facing == Face.Front){
				currentAnimator.SetBool("DropBegin", true);

			}
			break;

		case PlayingAnim.TurnAway:
			if(facing == Face.Left || facing == Face.Right ){

				Back.transform.localScale = new Vector3(1, 1, 1);
				currentAnimator.SetBool("Talking", false);
				currentAnimator.SetBool("Walking", false);
				currentAnimator.SetBool("Laughing", false);
				currentAnimator.SetInteger("PickUp", 0);
				currentAnimator.SetBool("TightRope", false);
				currentAnimator.SetBool("TurnLeft", false);
				currentAnimator.SetBool("Idle",true);
				//facing=Face.Back;

				StartCoroutine(TurnBack());
			}
			break;



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
				currentAnimator.SetBool("LightLamp", true);
				action = PlayingAnim.Idle;
			}
			break;
		}
	}
}
