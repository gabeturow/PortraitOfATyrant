using UnityEngine;
using System.Collections;

public class TapInteractor : MonoBehaviour {
	public static TapInteractor main;
	public bool wasPressed; 
	public CharacterMotor characterToMove;
	ParticleSystem tapParticles;
	Camera mainCam;

	Interactable queuedInteraction;

	void Awake(){
		main = this;
		mainCam = Camera.main;
		tapParticles = GetComponentInChildren<ParticleSystem>();
		tapParticles.GetComponent<Renderer>().sortingLayerName = "Top";
		tapParticles.GetComponent<Renderer>().sortingOrder = 999;
	}

	// Update is called once per frame
	void Update () {

		 wasPressed = Input.GetMouseButtonDown(0);
		if (wasPressed){
			MovementInteractionUpdate();
		}

		QueuedInteractionUpdate();

	}

	void MovementInteractionUpdate(){
		if (DialogueViewer.main.currentDialogue != null || UIMan.main.IsPanelActive() || GameMan.main.conditionals.GetValue("CUTSCENERUNNING")) return;

		queuedInteraction = null;
		Vector3 mouseRayPos = Input.mousePosition;
		mouseRayPos.z = -mainCam.transform.position.z;
		Vector3 touchPos = mainCam.ScreenToWorldPoint(mouseRayPos);
		Vector2 screenTouchPos = mainCam.ScreenToViewportPoint(mouseRayPos);
		var hitInfo = Physics2D.Raycast(touchPos, Vector3.zero);

		tapParticles.Emit(new Vector3(touchPos.x, touchPos.y, characterToMove.transform.position.z), Vector2.zero, 1f, 1f, Color.white);

		//touching UI
		if (screenTouchPos.x < .05 || screenTouchPos.x > .90f){ return; }


		if (hitInfo.collider != null){
			Debug.Log(hitInfo.collider.gameObject.name);
			var interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
			if (interactable != null && interactable.CanInteract()){
				if (interactable.WithinInteractionDistance(characterToMove.gameObject)){
					//MovePlayer(interactable.MinimalInteractionTarget(characterToMove.gameObject));

					interactable.Interact();
					//MovePlayer(touchPos/10);
				}
				else{
					queuedInteraction = interactable;
					MovePlayer(interactable.MinimalInteractionTarget(characterToMove.gameObject));
					//MovePlayer(touchPos);
				}
			}
		}
		else{
			MovePlayer(touchPos);
			InventoryMan.flyoutMenu.Hide();
		}
	}


	void QueuedInteractionUpdate(){
		if (queuedInteraction != null){
			Vector2 targetPos = queuedInteraction.transform.position;
			targetPos.y = characterToMove.transform.position.y;
			if (queuedInteraction.WithinInteractionDistance(characterToMove.gameObject) 
				&& characterToMove.currentVelocity.sqrMagnitude < .2f){
				queuedInteraction.Interact();
				queuedInteraction = null;
			}
		}
	}


	void MovePlayer(Vector3 touchPos){
		Toaster.main.Hide();
		characterToMove.target = new Vector2(touchPos.x, characterToMove.target.y);
	}

}
