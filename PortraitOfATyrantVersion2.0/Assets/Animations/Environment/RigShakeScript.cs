using UnityEngine;
using System.Collections;

public class RigShakeScript : MonoBehaviour {
	public enum rigstat{
		Idle, Shaking
	}
	public rigstat RigState;
	private Animator myAnim;

	// Use this for initialization
	void Start () {
		myAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (RigState){
		case rigstat.Idle:
			
			break;
		case rigstat.Shaking:
			myAnim.SetTrigger("Shake");
			RigState = rigstat.Idle;
			break;
		}
	}
}
