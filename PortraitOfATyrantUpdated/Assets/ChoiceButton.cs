using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChoiceButton : MonoBehaviour, ISelectHandler {

	public DialogueChoice choice{get; private set;}
	System.Action onSubmit;

	public Text t;
	public Image img1;

	void Awake(){
//		choicePanel= GetComponentInChildren<ChoicePanel>();
	}

	public void Init(DialogueChoice dialogueChoice, System.Action onSubmit){
		this.choice = dialogueChoice;
/*		if(choicePanel.grievanceActive){
			dialoguePanel.SetActive(false);
			grievancePanel.SetActive(true);
		}
		if(dialoguePanel.SetActive(true);
			
			grievancePanel.SetActive(false);
		}
		*/
		if(choice.grievanceImage!=null && img1!=null){
		img1.sprite=choice.grievanceImage;
		}

		t.text = choice.responseText;
		this.onSubmit = onSubmit;
	}

	public void OnSelect(BaseEventData data){
		if (onSubmit != null){
			onSubmit();
		}
	}

	void Update(){
		this.transform.localScale = new Vector3(1f,1f,1f);
	}

}
