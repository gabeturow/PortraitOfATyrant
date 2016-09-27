using UnityEngine;
using System.Collections;

public class DialogueTester : MonoBehaviour {

	public DialogueViewer v;
	public DialogueTree t;


	void Update(){
		if (Input.anyKeyDown && v.currentDialogue == null){
			v.PlayDialogue(t);
		}
	}


}
