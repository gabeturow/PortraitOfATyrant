using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class DialogueModule : InteractionModule {

	[SerializeField]
	DialogueTree dialogue;

	[SerializeField]
	float delayTime=0;

	void PlayDialogue(){
		DialogueViewer.main.PlayDialogue(dialogue);
	}
	public override void OnInteract ()
	{
		Invoke("PlayDialogue",delayTime);

	}

}
