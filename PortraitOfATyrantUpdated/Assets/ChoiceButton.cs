using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChoiceButton : MonoBehaviour, ISelectHandler {

	public DialogueChoice choice{get; private set;}

	System.Action onSubmit;

	public Text t;
	public Image img1;

	public void Init(DialogueChoice dialogueChoice, System.Action onSubmit){
		this.choice = dialogueChoice;
		Debug.Log(choice.responseText);
		Debug.Log(t.text);
		if(choice.grievanceImage!=null){
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
