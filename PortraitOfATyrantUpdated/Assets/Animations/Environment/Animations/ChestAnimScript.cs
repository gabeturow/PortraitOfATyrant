using UnityEngine;
using System.Collections;

public class ChestAnimScript : InteractionModule {

	private Animator chestAnim;
	public enum cState{
		Idle, Open, Close
	}
	public cState ChestState;

	// Use this for initialization
	void Start () {
		chestAnim = GetComponent<Animator>();
	}

	// Update is called once per frame
	public override void OnInteract(){

		if(!GameMan.main.conditionals.GetValue("CHESTOPEN")){
	
			chestAnim.SetTrigger("Open");
			ChestState = cState.Idle;
	
		}else{
	
			chestAnim.SetTrigger("Close");
			ChestState = cState.Idle;
	
		}
	}
}
