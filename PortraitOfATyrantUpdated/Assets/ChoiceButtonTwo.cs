using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChoiceButtonTwo : MonoBehaviour, ISelectHandler {

	public DialogueChoice choice{get; private set;}
	System.Action onSubmit;

	public Text t;
	public Text tLabel;
	public Image img1Two;

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
		Debug.Log(choice.responseText);
//		Debug.Log(tTwo.text);
		if(choice.grievanceImage!=null && img1Two!=null){
			img1Two.sprite=choice.grievanceImage;
		}
	
		if(choice.label!=null){
		tLabel.text = choice.label;
		}
		t.text = choice.responseText;
		this.onSubmit = onSubmit;
	}


	public void OnSelect(BaseEventData data){
		Invoke("WaitToSubmit",.5f);

	}
	void WaitToSubmit(){
	if (onSubmit != null){
		onSubmit();
	}
	}

	void Update(){
		this.transform.localScale = new Vector3(1f,1f,1f);
	}

}