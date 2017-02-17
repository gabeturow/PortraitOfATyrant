using UnityEngine;
using System.Collections;

public class MoveMouthWithAudio : MonoBehaviour {
	SpectrumAnalysis spectrumAnalysis;
	public Transform mouthToMove;
	public string currentName;
	AudioSource audioSourceDialogue;
	public float voiceAmplitude;
	public float multiplyMovementNew=1;
	// Use this for initialization
	void Start () {
		audioSourceDialogue=DialogueViewer.main.audioSource;
		spectrumAnalysis=DialogueViewer.main.GetComponent<SpectrumAnalysis>();
		mouthToMove=this.transform;
	}
	
	// Update is called once per frame
	void Update () {

		if(DialogueViewer.main.characterName.text==currentName && DialogueViewer.main.characterName.text!="Lasarus"){
			voiceAmplitude=-spectrumAnalysis.rmsValue*1000;
			if(voiceAmplitude<-60){
				voiceAmplitude/=(5/2);
			}else if(voiceAmplitude<-30){
				voiceAmplitude/=2;
			}
				mouthToMove.localRotation = Quaternion.Euler(0.0f,0.0f,multiplyMovementNew*voiceAmplitude);
		}




		if(spectrumAnalysis.rmsValue==0){
			mouthToMove.localRotation = Quaternion.Euler(0.0f,0.0f,0);
		}
	}
}
