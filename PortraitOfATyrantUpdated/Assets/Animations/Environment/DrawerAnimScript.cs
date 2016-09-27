using UnityEngine;
using System.Collections;

public class DrawerAnimScript : MonoBehaviour {
	
	private Animator drawerAnim;
	public enum cState{
		Idle, Open, Close
	}
	public cState DrawerState;

	// Use this for initialization
	void Start () {
		drawerAnim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		switch (DrawerState){
		case cState.Open:
			drawerAnim.SetTrigger("Open");
			DrawerState = cState.Idle;
			break;
		case cState.Close:
			drawerAnim.SetTrigger("Close");
			DrawerState = cState.Idle;
			break;
		}
	}
}
