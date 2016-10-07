using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceButtonThree : MonoBehaviour, ISelectHandler {

	public DialogueChoice choice{get; private set;}
	System.Action onSubmit;

	public Text ttt;
	public Text ttLabel;
	public Image img11Two;
	public Image rightsImageChoice;
	public Text rightsStringChoice;



	void Awake(){

	}

	public void Init(DialogueChoice dialogueChoice, System.Action onSubmit){
		this.choice = dialogueChoice;
	
		Debug.Log(choice.responseText);

		if(choice.grievanceImage!=null && img11Two!=null){
			img11Two.sprite=choice.grievanceImage;
		}

		if(choice.label!=null){
			ttLabel.text = choice.label;
		}


		rightsImageChoice.sprite = DialogueNode.rightsImageMenu;
		rightsStringChoice.text=DialogueNode.rightsLabelMenu;

		ttt.text = choice.responseText;
		this.onSubmit = onSubmit;
	}


	public void OnSelect(BaseEventData data){
		Invoke("WaitToSubmit",.25f);

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