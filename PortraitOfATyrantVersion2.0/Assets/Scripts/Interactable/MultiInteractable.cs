using UnityEngine;
using System.Collections;

public class MultiInteractable : Interactable {

	[SerializeField]
	Interactable[] interactables;

	public override bool CanInteract ()
	{
		bool canInteract = true;
		for(int i = 0; i < interactables.Length; i++){
			canInteract = canInteract || interactables[i].CanInteract();
		}
		return canInteract;
	}

	protected override void InternalInteract ()
	{
		base.InternalInteract ();
		for(int i = 0; i < interactables.Length; i++){
			if (interactables[i].CanInteract()){
				interactables[i].Interact();
				return;
			}
		}
	}

}
