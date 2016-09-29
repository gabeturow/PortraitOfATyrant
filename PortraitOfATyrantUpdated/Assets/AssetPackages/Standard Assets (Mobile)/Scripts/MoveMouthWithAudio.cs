using UnityEngine;
using System.Collections;

public class MoveMouthWithAudio : MonoBehaviour {
	SpectrumAnalysis spectrumAnalysis;
	public Transform mouthToMove;
	public string currentName;
	AudioSource audioSourceDialogue;
	// Use this for initialization
	void Start () {
		audioSourceDialogue=DialogueViewer.main.audioSource;
		spectrumAnalysis=DialogueViewer.main.GetComponent<SpectrumAnalysis>();

	}
	
	// Update is called once per frame
	void Update () {
		if(DialogueViewer.main.characterName.text==currentName){
			mouthToMove.localRotation = Quaternion.Euler(0.0f,0.0f,-spectrumAnalysis.rmsValue*1000);
		}
	}
}
