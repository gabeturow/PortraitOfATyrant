using UnityEngine;
using System.Collections;

public class BeginDialogueOnStart :InteractionModule {


		[SerializeField]
		DialogueTree dialogue;
	[SerializeField]
	float timeToInvoke=0f;

	bool stopDialogue=false;

	void Start(){
		Invoke("DoIt", timeToInvoke);
	}

	void DoIt(){
		if(!stopDialogue){
		DialogueViewer.main.PlayDialogue(dialogue);
		}

	}

	public void StopDialogue(){
		stopDialogue=true;
			DialogueViewer.main.StopNow();

	}

}

