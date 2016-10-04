using UnityEngine;
using System.Collections;

public class CompressorGain : MonoBehaviour {

	public float desiredVolume;
	SpectrumAnalysis spectrum;
	RoomMan room1;
	// Use this for initialization
	void Start () {
		spectrum=GetComponent<SpectrumAnalysis>();

	}
	
	// Update is called once per frame
	void Update () {
	/*	if(RoomMan.main.current.name=="BreadRoom(Clone)"){
			gameObject.GetComponent<AudioSource>().volume=.2f;
		}else if(RoomMan.main.current.name=="UpperDeck 1(Clone)"){
			gameObject.GetComponent<AudioSource>().volume=1f;
		}else{
			gameObject.GetComponent<AudioSource>().volume=.5f;
		}*/
		Debug.Log(gameObject.GetComponent<AudioSource>().volume);
	}
}
