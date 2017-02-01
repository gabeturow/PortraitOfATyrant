using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

	public float interactionDistance = 3;

	[SerializeField]
	public float interactionBuffer = 2f;
	float interactionBufferTimer =0;

	private InteractionModule[] _modules;
	protected InteractionModule[] modules{
		get{
			if (_modules == null){
				_modules = new InteractionModule[0];
				_modules = GetComponents<InteractionModule>();
			}
			return _modules;
		}
	}

	public virtual bool CanInteract(){
		bool canInteract = true;
		for(int i = 0; i < modules.Length; i++){
			canInteract = canInteract && modules[i].CheckInteract();
		}
		return canInteract;
	}

	public float Distance(GameObject other){
		var thisPosition = new Vector2(transform.position.x, other.transform.position.y);
		return Vector2.Distance(thisPosition, other.transform.position);
	}

	public bool WithinInteractionDistance(GameObject other){
		return Distance(other) < interactionDistance;
	}

	public Vector2 MinimalInteractionTarget(GameObject other){
		if (Mathf.Abs(other.transform.position.x - this.transform.position.x) < interactionDistance) return other.transform.position;
		var delta = this.transform.position - other.transform.position;
		return this.transform.position - delta.normalized * interactionDistance * .25f;
	}

	public virtual void Interact(){
		if (interactionBufferTimer > 0){
			return;
		}

		InternalInteract();

		interactionBufferTimer = interactionBuffer;
	}

	protected virtual void InternalInteract(){
		for(int i = 0; i < modules.Length; i++){
			modules[i].OnInteract();
		}
	}

	protected virtual void Update(){
		if (interactionBufferTimer > 0){
			interactionBufferTimer -= Time.deltaTime;
		}
	}
}
