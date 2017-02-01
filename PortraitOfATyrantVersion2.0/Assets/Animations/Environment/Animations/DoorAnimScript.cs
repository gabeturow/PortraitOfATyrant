using UnityEngine;
using System.Collections;

public class DoorAnimScript : MonoBehaviour {
	private Animator doorAnim;
	public enum dState{
		Idle, Open, Close
	}
	public dState DoorState;

	// Use this for initialization
	void Start () {
		doorAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (DoorState){
		case dState.Open:
			doorAnim.SetTrigger("Open");
			DoorState = dState.Idle;
			break;
		case dState.Close:
			doorAnim.SetTrigger("Close");
			DoorState = dState.Idle;
			break;
		}
	}
}
