using UnityEngine;
using System.Collections;

namespace Slate.ActionClips{

	[Name("Audio Clip")]
	[Description("The audio clip will be send to the AudioMixer selected in it's track if any. You can trim or loop the audio by scaling the clip and you can optionaly show subtitles as well.")]
	[Attachable(typeof(ActorAudioTrack), typeof(DirectorAudioTrack))]
	public class PlayAudio : ActionClip, ISubClipContainable {

		[SerializeField] [HideInInspector]
		private float _length = 1f;
		[SerializeField] [HideInInspector]
		private float _blendIn = 0.25f;
		[SerializeField] [HideInInspector]
		private float _blendOut = 0.25f;
		
		[Required]
		public AudioClip audioClip;
		public float clipOffset;
		[Multiline(5)]
		public string subtitlesText;
		public Color subtitlesColor = Color.white;

		float ISubClipContainable.subClipOffset{
			get {return clipOffset;}
			set {clipOffset = value;}
		}

		public override float length{
			get { return _length;}
			set	{_length = value;}
		}

		public override float blendIn{
			get {return _blendIn;}
			set {_blendIn = value;}
		}

		public override float blendOut{
			get {return _blendOut;}
			set {_blendOut = value;}
		}

		public override bool isValid{
			get {return audioClip != null;}
		}

		public override string info{
			get { return isValid? (string.IsNullOrEmpty(subtitlesText)? audioClip.name : string.Format("<i>'{0}'</i>", subtitlesText) ): base.info; }
		}

		private AudioSource source{
			get {return AudioSampler.GetSourceForID(parent);}
		}

	
		protected override void OnEnter(){
			source.clip = audioClip;
		}

		protected override void OnUpdate(float time, float previousTime){
			var weight = Easing.Ease(EaseType.QuadraticInOut, 0, 1, GetClipWeight(time));
			var totalVolume = weight * (parent as AudioTrack).weight; //put 'weight' in interface?
			
			AudioSampler.SampleForID(parent, audioClip, time - clipOffset, previousTime - clipOffset, totalVolume);

			if (!string.IsNullOrEmpty(subtitlesText)){
				var lerpColor = subtitlesColor;
				lerpColor.a = weight;
				DirectorGUI.UpdateSubtitles(string.Format("{0}{1}", parent is ActorAudioTrack? (actor.name + ": ") : "", subtitlesText), lerpColor);
			}
		}

		protected override void OnExit(){
			source.clip = null;
		}

		protected override void OnReverse(){
			source.clip = null;
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnClipGUI(Rect rect){
			EditorTools.DrawLoopedAudioTexture(rect, audioClip, length, clipOffset);
		}			

		#endif
	}
}