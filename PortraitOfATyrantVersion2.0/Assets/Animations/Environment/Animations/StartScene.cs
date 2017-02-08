using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {
	private Animator cameraAnim;
	public enum dState{
		Go, Idle
	}
	public dState CameraState;

	// Use this for initialization
	void Start () {
		cameraAnim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		switch (CameraState){
		case dState.Go:
			cameraAnim.SetTrigger("Go");
			CameraState = dState.Idle;
			break;
		case dState.Idle:
			cameraAnim.SetTrigger("Idle");
			CameraState = dState.Idle;
			break;
		}
	}
}
