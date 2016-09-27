using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class ToastModule : InteractionModule {

	[SerializeField]
	[TextArea(2, 4)]
	protected string _message;
	public int delay=0;

	public string Message{
		get{ return _message; } 
	}

	IEnumerator GoToast(){
		yield return new WaitForSeconds(delay);
		Toaster.main.Show(_message);
	}

	public override void OnInteract ()
	{
		if (_message != ""){
			if(delay==0){
				Toaster.main.Show(_message);
			}else{

			StartCoroutine(GoToast());
			}
		}

	}
}
