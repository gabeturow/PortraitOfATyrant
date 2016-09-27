using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ChoicePanel : MonoBehaviour {

	ChoiceButton template;

	ChoiceButton[] buttons = new ChoiceButton[0];

	ManualLocalScaleSpring scaleSpring;

	public bool isActive = false;
	public bool grievanceActive=false;
	public bool rightsActive=false;
	public bool riddleActive=false;
	public Text riddleText;
	public GameObject grievancetextobject;
	public GameObject riddletextobject;
	public GameObject rightstextobject;


	void Awake(){
		template = GetComponentInChildren<ChoiceButton>();
		template.gameObject.SetActive(false);
		scaleSpring = this.gameObject.ForceGetComponent<ManualLocalScaleSpring>();
		scaleSpring.dampingRatio = .5f;

	}
void Update(){
		//riddleText.text=DialogueViewer.currentDialogue.CurrentMessage.text;
		//THIS CHANGES THE TEXT FOR THE SELECTION INTERFACE PROMPTS
		if(grievanceActive){
			grievancetextobject.SetActive(true);
			riddletextobject.SetActive(false);
			rightstextobject.SetActive(false);
		}
		if(rightsActive){
			grievancetextobject.SetActive(false);
			riddletextobject.SetActive(false);
			rightstextobject.SetActive(true);
		}
		if(riddleActive){
			grievancetextobject.SetActive(false);
			riddletextobject.SetActive(true);
			rightstextobject.SetActive(false);
		}
	

		scaleSpring.target = isActive ? Vector3.one : new Vector3(1f, 0f, 1f);
		scaleSpring.UpdateMe();
	}

	public void ClearChoices(){
		for (int i = 0; i < buttons.Length; i++){
			Destroy(buttons[i].gameObject);
		}
		buttons = new ChoiceButton[0];
	}

	public void SetChoices(DialogueChoice[] choices, System.Action<int> onSubmit){

		ClearChoices();

		buttons = new ChoiceButton[choices.Length];
		for(int i = 0; i < choices.Length; i++){
			buttons[i] = ((GameObject)Instantiate(template.gameObject)).GetComponent<ChoiceButton>();
			buttons[i].gameObject.transform.parent = template.transform.parent;
			var index = i;
			buttons[i].Init(choices[i], ()=>{onSubmit(index);});
			buttons[i].gameObject.SetActive(true);
		}

	}

}
