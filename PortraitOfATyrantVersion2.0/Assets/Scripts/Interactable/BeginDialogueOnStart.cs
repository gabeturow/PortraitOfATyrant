using UnityEngine;
using System.Collections;

public class BeginDialogueOnStart :InteractionModule {


		[SerializeField]
		DialogueTree dialogue;
	[SerializeField]
	float timeToInvoke=0f;
	void Start(){
		Invoke("DoIt", timeToInvoke);
	}

	void DoIt(){
		DialogueViewer.main.PlayDialogue(dialogue);
	}

}

