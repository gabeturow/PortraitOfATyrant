using UnityEngine;
using System.Collections;

public class SwitchChoicePanel : MonoBehaviour {

	ChoicePanel choicePanel;
	public GameObject grievancePanel;
	public GameObject dialoguePanel;


	void Awake(){
		choicePanel= GetComponentInChildren<ChoicePanel>();
	}
	// Update is called once per frame
	void Update () {
		
			
	}
}
