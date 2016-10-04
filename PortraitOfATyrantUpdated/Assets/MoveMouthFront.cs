using UnityEngine;
using System.Collections;

public class MoveMouthFront : MonoBehaviour {

		SpectrumAnalysis spectrumAnalysis;
		public Transform mouthToMove;
		public string currentName;
		AudioSource audioSourceDialogue;
		public float voiceAmplitude;
	public Vector3 startMouthPosition;
		// Use this for initialization
		void Start () {
			audioSourceDialogue=DialogueViewer.main.audioSource;
			spectrumAnalysis=DialogueViewer.main.GetComponent<SpectrumAnalysis>();
		startMouthPosition.y=mouthToMove.localPosition.y;
		startMouthPosition.x=mouthToMove.localPosition.x;

		}

		// Update is called once per frame
		void Update () {


		if(DialogueViewer.main.characterName.text==currentName){
			voiceAmplitude=-spectrumAnalysis.rmsValue*1;
			if(voiceAmplitude<-60){
				voiceAmplitude/=(5/2);
			}else if(voiceAmplitude<-30){
				voiceAmplitude/=2;
			}
			mouthToMove.localPosition = new Vector3((startMouthPosition.x),startMouthPosition.y+voiceAmplitude,0);
		}

			if(spectrumAnalysis.rmsValue==0){
				mouthToMove.localRotation = Quaternion.Euler(0.0f,0.0f,0);
			}
		}
	}
