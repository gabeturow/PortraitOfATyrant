#if UNITY_5_4_OR_NEWER

using UnityEngine;
using UnityEngine.Experimental.Director;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Slate.ActionClips;

namespace Slate{

	[UniqueElement] //one per group until layer playable release
	[Description("The Animator Track works with an 'Animator' Component attached on the actor, but does not require or use the Controller assigned. Instead animation clips can be played directly. The 'Base Animation Clip' will be played along the whole track length when no other animation clip is playing. This can usualy be something like an Idle.")]
	[Icon("Animator Icon")]
	[Attachable(typeof(ActorGroup))]
	public class AnimatorTrack : CutsceneTrack {

		const int ROOTMOTION_FRAMERATE = 30;

		public AnimationClip baseAnimationClip;
		public bool useRootMotion = false;

		private Animator animator;
		private AnimationMixerPlayable mixerPlayable;
		private Dictionary<PlayAnimatorClip, int> ports;
		private int activeClips;
		private bool useBakedRootMotion;
		private List<Vector3> positions;
		private List<Quaternion> rotations;

		private RuntimeAnimatorController wasController;
		private AnimatorCullingMode wasCullingMode;
		private bool wasRootMotion;
		private bool wasEnabled;

		public override string info{
			get {return string.Format("Base Clip: {0}", baseAnimationClip? baseAnimationClip.name : "NONE");}
		}


		//...
		protected override bool OnInitialize(){
			animator = actor.GetComponent<Animator>();
			if (animator == null){
				Debug.LogError("Animator Track requires that the actor has the Animator Component attached.", actor);
				return false;
			}

			return true;
		}


		//Create playable tree
		void CreateAndPlayTree(){
			ports = new Dictionary<PlayAnimatorClip, int>();
			mixerPlayable = AnimationMixerPlayable.Create();

			var basePlayableClip = AnimationClipPlayable.Create(baseAnimationClip);
			basePlayableClip.state = PlayState.Paused;
			mixerPlayable.AddInput(basePlayableClip);

			foreach(var playAnimClip in actions.OfType<PlayAnimatorClip>()){
				var playableClip = AnimationClipPlayable.Create(playAnimClip.animationClip);
				playableClip.state = PlayState.Paused;
				// playableClip.applyFootIK = true;
				var index = mixerPlayable.AddInput(playableClip);
				mixerPlayable.SetInputWeight(index, 0f);
				ports[playAnimClip] = index;
			}

			animator.SetTimeUpdateMode(DirectorUpdateMode.Manual);
			animator.Play(mixerPlayable);
			mixerPlayable.state = PlayState.Paused;
		}


		//The root motion only must be baked if required.
		void BakeRootMotion(){
			useBakedRootMotion = false;
			animator.applyRootMotion = true;
			positions = new List<Vector3>();
			rotations = new List<Quaternion>();
			var clips = (this as IDirectable).children;
			var lastTime = -1f;
			var updateInterval = (1f/ROOTMOTION_FRAMERATE);
			var tempActiveClips = 0;
			for (var i = startTime; i <= endTime + updateInterval; i += updateInterval){
				foreach(var clip in clips){

					if (i >= clip.startTime && lastTime < clip.startTime){
						tempActiveClips++;
						clip.Enter();
					}

					if (i >= clip.startTime && i <= clip.endTime){
						clip.Update(i - clip.startTime, i - clip.startTime - updateInterval);
					}

					if ( (i > clip.endTime || i >= endTime) && lastTime <= clip.endTime){
						tempActiveClips--;
						clip.Exit();
					}
				}

				if (tempActiveClips > 0){
					animator.Update( updateInterval );
				}

				positions.Add(actor.transform.localPosition);
				rotations.Add(actor.transform.localRotation);
				lastTime = i;
			}
			animator.applyRootMotion = false;
			useBakedRootMotion = true;
		}

		//Apply baked root motion by lerping between stored frames.
		void ApplyBakedRootMotion(float time){
			var frame = Mathf.FloorToInt( time * ROOTMOTION_FRAMERATE );
			var nextFrame = frame + 1;

			if (frame < positions.Count && nextFrame < positions.Count){
				var tNow = frame * (1f/ROOTMOTION_FRAMERATE);
				var tNext = nextFrame * (1f/ROOTMOTION_FRAMERATE);
			
				var posNow = positions[frame];
				var posNext = positions[nextFrame];
				var pos = Vector3.Lerp(posNow, posNext, Mathf.InverseLerp(tNow, tNext, time) );
				actor.transform.localPosition = pos;

				var rotNow = rotations[frame];
				var rotNext = rotations[nextFrame];
				var rot = Quaternion.Lerp(rotNow, rotNext, Mathf.InverseLerp(tNow, tNext, time) );
				actor.transform.localRotation = rot;
			}
		}

		protected override void OnUpdate(float time, float previousTime){

			if (animator == null){
				return;
			}

			if (baseAnimationClip != null){
				var basePlayable = mixerPlayable.GetInput(0);
				basePlayable.time = time;
				mixerPlayable.SetInput(basePlayable, 0);
			}

			animator.Update(0);

			if (useRootMotion && useBakedRootMotion){
				ApplyBakedRootMotion(time);
			}
		}

		public void EnableClip(PlayAnimatorClip playAnimClip){
			
			if (animator == null){
				return;
			}

			activeClips++;
			var index = ports[playAnimClip];
			var weight = playAnimClip.GetClipWeight();
			mixerPlayable.SetInputWeight(0, activeClips == 2? 0 : 1 - weight);
			mixerPlayable.SetInputWeight(index, weight);
		}

		public void DisableClip(PlayAnimatorClip playAnimClip){
			
			if (animator == null){
				return;
			}

			activeClips--;
			var index = ports[playAnimClip];
			mixerPlayable.SetInputWeight(0, activeClips == 0? 1 : 0);
			mixerPlayable.SetInputWeight(index, 0);
		}


		public void UpdateClip(PlayAnimatorClip playAnimClip, float clipTime, float clipPrevious, float weight){

			if (animator == null){
				return;
			}			

			var index = ports[playAnimClip];
			var clipPlayable = mixerPlayable.GetInput(index);
			clipPlayable.time = clipTime;
			mixerPlayable.SetInput(clipPlayable, index);
			mixerPlayable.SetInputWeight(index, weight);
			mixerPlayable.SetInputWeight(0, activeClips == 2? 0 : 1f - weight );
		}


		void StoreSet(){
			wasController = animator.runtimeAnimatorController;
			wasRootMotion = animator.applyRootMotion;
			wasCullingMode = animator.cullingMode;
			wasEnabled = animator.enabled;

			animator.applyRootMotion = false;
			animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			animator.enabled = false;
		}

		void Restore(){
			if (animator != null){
				animator.runtimeAnimatorController = wasController;
				animator.applyRootMotion = wasRootMotion;
				animator.cullingMode = wasCullingMode;
				animator.enabled = wasEnabled;			
				//mixerPlayable.Destroy(); //crashes unity
			}			
		}
		
		protected override void OnEnter(){

			animator = actor.GetComponent<Animator>(); //re-get to fetch from virtual actor ref instance if any
			if (animator == null){
				return;
			}

			StoreSet();
			CreateAndPlayTree();
			if (useRootMotion){
				BakeRootMotion();
			}
		}

		protected override void OnReverseEnter(){

			animator = actor.GetComponent<Animator>(); //re-get to fetch from virtual actor ref instance if any
			if (animator == null){
				return;
			}

			StoreSet();
			CreateAndPlayTree();
			//DO NOT Re-Bake root motion
		}

		protected override void OnExit(){ Restore(); }
		protected override void OnReverse(){ Restore(); }

	}
}

#endif