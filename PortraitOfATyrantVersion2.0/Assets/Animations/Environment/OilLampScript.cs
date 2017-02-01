using UnityEngine;
using System.Collections;

public class OilLampScript : MonoBehaviour {
	public bool Lit;
	private Animator myAnim;

	// Use this for initialization
	void Start () {
		myAnim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		switch (Lit){
		case true:
			myAnim.SetBool("Lit", true);

			break;
		case false:
			myAnim.SetBool("Lit", false);

			break;
	}
	}
}
