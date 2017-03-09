using UnityEngine;
using System.Collections;

public class ToastThoughts : MonoBehaviour {

	public static ToastThoughts main;
	public AudioSource toastBroadcaster;

	void Awake () {
		toastBroadcaster=this.gameObject.GetComponent<AudioSource>();
	}

}
