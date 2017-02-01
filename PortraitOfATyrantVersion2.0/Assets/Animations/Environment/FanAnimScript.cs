using UnityEngine;
using System.Collections;

public class FanAnimScript : MonoBehaviour {

	private Animator fanAnim;
	public enum cState{
		Idle, Open, Close
	}
	public cState FanState;

	// Use this for initialization
	void Start () {
		fanAnim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		switch (FanState){
		case cState.Open:
			fanAnim.SetTrigger("Open");
			FanState = cState.Idle;
			break;
		case cState.Close:
			fanAnim.SetTrigger("Close");
			FanState = cState.Idle;
			break;
		}
	}
}
