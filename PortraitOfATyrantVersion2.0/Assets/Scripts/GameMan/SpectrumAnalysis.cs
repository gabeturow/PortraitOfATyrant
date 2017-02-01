using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;

		public class SpectrumAnalysis : MonoBehaviour {
	public static SpectrumAnalysis main;
		public int qSamples = 1024;  // array size
		public float refValue = 0.1f; // RMS value for 0 dB
		public float threshold = 0.02f;  // minimum amplitude to extract pitch
		public float rmsValue;  // sound level - RMS
		public float dbValue;  // sound level - dB
		public float pitchValue; // sound pitch - Hz
		private float[] samples; // audio samples
		private float[] spectrum; // audio spectrum
		private float fSample;

		void Start () {

			samples = new float[qSamples];
			spectrum = new float[qSamples];
			fSample = AudioSettings.outputSampleRate;
		}

		void AnalyzeSound(){
			GetComponent<AudioSource>().GetOutputData(samples, 0); // fill array with samples
			int i;
			float sum = 0.0f;

			for (i=0; i < qSamples; i++){
				sum += samples[i]*samples[i]; // sum squared samples
			}
			rmsValue = Mathf.Sqrt(sum/qSamples); // rms = square root of average
			dbValue = 20*Mathf.Log10(rmsValue/refValue); // calculate dB

			if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
			// get sound spectrum

			GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
			float maxV = 0.0f;
			int maxN = 0;

			for (i=0; i < qSamples; i++){ // find max
				if (spectrum[i] > maxV && spectrum[i] > threshold){
					maxV = spectrum[i];
					maxN = i; // maxN is the index of max
				}
			}
			float freqN = maxN; // pass the index to a float variable
			if (maxN > 0 && maxN < qSamples-1){ // interpolate index using neighbours
				var dL = spectrum[maxN-1]/spectrum[maxN];
				var dR = spectrum[maxN+1]/spectrum[maxN];
				freqN += 0.5f*(dR*dR - dL*dL);
			}
			pitchValue = freqN*(fSample/2)/qSamples; // convert index to frequency
		}

		public Text display; // drag a GUIText here to show results

		void Update () {
			if (Input.GetKeyDown("p")){
				GetComponent<AudioSource>().Play();
			}
			AnalyzeSound();
			if (display){
				display.text = "RMS: "+rmsValue.ToString("F2")+
					" ("+dbValue.ToString("F1")+" dB)\n"+
					"Pitch: "+pitchValue.ToString("F0")+" Hz";
			}
		}
	}