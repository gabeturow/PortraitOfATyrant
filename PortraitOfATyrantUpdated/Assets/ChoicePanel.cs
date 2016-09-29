using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ChoicePanel : MonoBehaviour {

	ChoiceButton template;
	ChoiceButtonTwo templateTwo;
	CanvasGroup canvasGroup;
	CanvasGroupFader fader;
	ChoiceButton[] buttons = new ChoiceButton[0];
	ChoiceButtonTwo[] buttonsTwo = new ChoiceButtonTwo[0];

	ManualLocalScaleSpring scaleSpring;

	public bool isActive = false;

	public Text riddleText;

	//public string promptText;


	void Awake(){
		template = GetComponentInChildren<ChoiceButton>();
		templateTwo = GetComponentInChildren<ChoiceButtonTwo>();
		template.gameObject.SetActive(false);
		templateTwo.gameObject.SetActive(false);
		scaleSpring = this.gameObject.ForceGetComponent<ManualLocalScaleSpring>();
		scaleSpring.dampingRatio = 1.2f;
		fader = gameObject.ForceGetComponent<CanvasGroupFader>();
	}
void Update(){
		//riddleText.text=DialogueViewer.currentDialogue.CurrentMessage.text;
		//THIS CHANGES THE TEXT FOR THE SELECTION INTERFACE PROMPTS
	

	//	scaleSpring.target = isActive ? Vector3.one : new Vector3(1f, 0f, 1f);
	//	scaleSpring.UpdateMe();
	}

	public void ClearChoices(){
		

		if(GameMan.main.conditionals.GetValue("GRIEVANCE")){
			for (int i = 0; i < buttonsTwo.Length; i++){
				Destroy(buttonsTwo[i].gameObject);
			}
			buttonsTwo = new ChoiceButtonTwo[0];
		}else{
			for (int i = 0; i < buttons.Length; i++){
				Destroy(buttons[i].gameObject);
			}
			buttons = new ChoiceButton[0];
		}

	}

	public void SetChoices(DialogueChoice[] choices, System.Action<int> onSubmit){
		ClearChoices();
		fader.displaying = true;
		if(GameMan.main.conditionals.GetValue("GRIEVANCE")){
		buttonsTwo = new ChoiceButtonTwo[choices.Length];
			for(int i = 0; i < choices.Length; i++){
				buttonsTwo[i] = ((GameObject)Instantiate(templateTwo.gameObject)).GetComponent<ChoiceButtonTwo>();
				buttonsTwo[i].gameObject.transform.parent = templateTwo.transform.parent;
				var index = i;

				buttonsTwo[i].Init(choices[i], ()=>{onSubmit(index);});
				buttonsTwo[i].gameObject.SetActive(true);
			}

		}else{

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

}
