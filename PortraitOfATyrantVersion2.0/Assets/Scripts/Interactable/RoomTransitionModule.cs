using UnityEngine;
using System.Collections;

public class RoomTransitionModule : InteractionModule {

	public Room connectedRoom;
	public string doorway;
	public bool shouldTurnLeft;
	public GameObject turnThisOff;

	public float delay = 1f;

	void Start(){

	}

	IEnumerator WaitToTransition(){

		//if(CharacterAnimation.action == ClimbUp){
		//	yield return new WaitForSeconds(6);
		//}else{
			yield return new WaitForSeconds(delay);
			//}
		Debug.Log("TRANSITION");
		if (connectedRoom == null) {
			Debug.Log("No connected room! Assign it in the inspector.");
		}

		RoomMan.main.LoadRoom(connectedRoom, doorway);
		TurnOffText();
		if(shouldTurnLeft){
			CharacterAnimation.main.TurnCharacterLeft(true);
		}

	}
	
	void TurnOffText(){
		if(turnThisOff!=null){
			turnThisOff.GetComponent<CanvasGroupFader>().displaying=false;
			//turnThisOff.SetActive(false);
			Debug.Log("TurnItOff");
		}
	}

	public override void OnInteract ()
	{


		StartCoroutine(WaitToTransition());


	}
}
