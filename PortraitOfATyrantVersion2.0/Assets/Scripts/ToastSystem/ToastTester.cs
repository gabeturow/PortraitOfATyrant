using UnityEngine;
using System.Collections;

public class ToastTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")){
			Toaster.main.Show(Random.value + "");
		}
		if (Input.GetKeyDown("h")){
			Toaster.main.Hide();
		}
	}
}
