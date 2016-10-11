using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ChoicePanel : MonoBehaviour {
	public static ChoicePanel main;
	GoBackButton goBackTemplate;
	ChoiceButton template;
	ChoiceButtonTwo templateTwo;
	ChoiceButtonThree templateThree;
	CanvasGroup canvasGroup;
	CanvasGroupFader fader;
	GoBackButton[] goBackButtons = new GoBackButton[0];
	ChoiceButton[] buttons = new ChoiceButton[0];
	ChoiceButtonTwo[] buttonsTwo = new ChoiceButtonTwo[0];
	ChoiceButtonThree[] buttonsThree = new ChoiceButtonThree[0];
	public static Sprite rightsImage;

	ManualLocalScaleSpring scaleSpring;

	public bool isActive = false;

	public Text riddleText;

	//public string promptText;


	void Awake(){
		goBackTemplate = GetComponentInChildren<GoBackButton>();
		template = GetComponentInChildren<ChoiceButton>();
		templateTwo = GetComponentInChildren<ChoiceButtonTwo>();
		templateThree = GetComponentInChildren<ChoiceButtonThree>();
		template.gameObject.SetActive(false);
		templateTwo.gameObject.SetActive(false);
		templateThree.gameObject.SetActive(false);
		goBackTemplate.gameObject.SetActive(false);
		scaleSpring = this.gameObject.ForceGetComponent<ManualLocalScaleSpring>();
		scaleSpring.dampingRatio = 1.2f;
		fader = gameObject.GetComponent<CanvasGroupFader>();
	}

void Update(){


		fader.displaying=isActive;
	}

	public void ClearChoices(){
		

		if(GameMan.main.conditionals.GetValue("GRIEVANCE")){
			for (int i = 0; i < buttonsTwo.Length; i++){
				Destroy(buttonsTwo[i].gameObject);
			}
			buttonsTwo = new ChoiceButtonTwo[0];
		
		}

		if(GameMan.main.conditionals.GetValue("RIGHTS")){
			for (int i = 0; i < buttonsThree.Length; i++){
				Destroy(buttonsThree[i].gameObject);
			}
			buttonsThree = new ChoiceButtonThree[0];
	
		}
		if(GameMan.main.conditionals.GetValue("DIALOGUE")){
			for (int i = 0; i < buttons.Length; i++){
				Destroy(buttons[i].gameObject);
			}
			buttons = new ChoiceButton[0];
		}




	}
	public void SetBackFunction(System.Action onSubmit){

		if(goBackButtons.Length>0){
			Destroy(goBackButtons[0].gameObject);
		}
		goBackButtons = new GoBackButton[1];
		goBackButtons[0] = ((GameObject)Instantiate(goBackTemplate.gameObject)).GetComponent<GoBackButton>();
		goBackButtons[0].gameObject.transform.parent = goBackTemplate.transform.parent;
		var index1 = 0;

		goBackButtons[0].Init(onSubmit);
		//}
		goBackButtons[0].gameObject.SetActive(true);
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
			//The go back Button


		}else if(GameMan.main.conditionals.GetValue("RIGHTS")){
			buttonsThree = new ChoiceButtonThree[choices.Length];
		for(int i = 0; i < choices.Length; i++){
			buttonsThree[i] = ((GameObject)Instantiate(templateThree.gameObject)).GetComponent<ChoiceButtonThree>();
			buttonsThree[i].gameObject.transform.parent = templateThree.transform.parent;
			var index = i;

			buttonsThree[i].Init(choices[i], ()=>{onSubmit(index);});
			buttonsThree[i].gameObject.SetActive(true);
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
