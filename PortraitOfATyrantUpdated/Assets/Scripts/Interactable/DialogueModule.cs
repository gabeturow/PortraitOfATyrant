using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class DialogueModule : InteractionModule {

	[SerializeField]
	DialogueTree dialogue;

	public override void OnInteract ()
	{
		DialogueViewer.main.PlayDialogue(dialogue);;
	}

}
