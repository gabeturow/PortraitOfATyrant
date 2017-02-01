using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoBackButton : MonoBehaviour, ISelectHandler {

	public DialogueChoice choice{get; private set;}
	System.Action onSubmit;

	public Text t;
	public Text tLabel;
	public Image img1Two;


	public void Init(System.Action onSubmit){
		
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