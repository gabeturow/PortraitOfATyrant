using UnityEngine;
using System.Collections;

namespace SamCinema{

	//base class
	public class Shot : SamAction{
		private float _smoothspeed = 1f;
		protected float smoothspeedmultiplier = .00001f;
		public float transitionTime = 1f;
		public float velocityModulator = .1f;
		public virtual float SmoothSpeed{get{return _smoothspeed * (1 / smoothspeedmultiplier);} protected set{_smoothspeed = value;}}
		public virtual Vector3 CameraPosition{get; protected set;}
		public Vector3 CameraForward{get; protected set;}

		GameObject _objectToTrack;
		public GameObject ObjectToTrack{get{return _objectToTrack;} set{_objectToTrack = value; if (_objectToTrack != null) objectLastPos = _objectToTrack.transform.position;}}
		public Vector3 trackingOffset = Vector3.zero;

		private GameObject _objectToFocus;
		public GameObject ObjectToFocus{get{return _objectToFocus ?? ObjectToTrack;}set{_objectToFocus = value;}}

		protected Vector3 objectCurrentPos{get{return ObjectToTrack.transform.position + ObjectToTrack.transform.InverseTransformVector(trackingOffset);}}
		public Vector3 objectPredictedPos{get{return objectCurrentPos + (objectVelocity / Time.deltaTime) * SmoothSpeed * velocityModulator;}}
//		public Vector3 objectPredictedPos{get{return objectCurrentPos;}}
		protected Vector3 objectLastPos;
		protected Vector3 objectVelocity = Vector3.zero;

		int velocityBuffer = 3;

		public Shot(){
			OnStart = ()=>{CameraController.main.currentShot = this;};
		}

		public Shot(Vector3 position, Vector3 forward) : this(){
			CameraPosition = position;
			CameraForward = forward;
			SmoothSpeed = 1f;
		}

		public Shot Smooth(float smoothspeed){
			this.SmoothSpeed = smoothspeed;
			return this;
		}
		public override void Update (){
			if (this.ObjectToTrack != null){
				smoothspeedmultiplier = Mathf.Clamp(smoothspeedmultiplier + Time.deltaTime / transitionTime, 0, 1);
				if (velocityBuffer > 0){
					velocityBuffer--;
				}
				else{
					objectVelocity = Vector3.Lerp(objectVelocity, objectCurrentPos - objectLastPos, .5f);
				}
				objectLastPos = objectCurrentPos;
			}
		}

		public Shot TransitionTime(float transitionTime){
			this.transitionTime = transitionTime;
			return this;
		}

		public Shot PredictVelocity(float scale){
			this.velocityModulator = scale;
			return this;
		}
	}


	//static camera doesn't move nor rotate.
	public class StaticShot : Shot{
		public StaticShot(Vector3 position, Vector3 forward) : base(position, forward){}
		public StaticShot(Vector3 position, Vector3 forward, float smoothTime) : base(position, forward){
			this.SmoothSpeed = smoothTime;
		}
	}

	//static camera is locked to world position (for transitions)
	public class StaticShotLock : StaticShot{
		private Vector3 worldPos;
		private bool usingWorldPos = false;
		public override Vector3 CameraPosition {
			get {
				return usingWorldPos ? worldPos : Camera.main.transform.position;
			}
			protected set {
				base.CameraPosition = value;
			}
		}
		public StaticShotLock(Vector3 worldPos, GameObject objectToTrack) : this(objectToTrack){
			this.worldPos = worldPos;
			usingWorldPos = true;
		}
		public StaticShotLock(Vector3 worldPos, Transform objectToTrack) : this(objectToTrack){
			this.worldPos = worldPos;
			usingWorldPos = true;
		}

		public StaticShotLock(GameObject objectToTrack) : base(Vector3.zero, Vector3.zero){
			this.ObjectToTrack = objectToTrack;
			OnStart = ()=>{CameraPosition = Camera.main.transform.position; CameraController.main.currentShot = this;};
		}
		public StaticShotLock(Transform objectToTrack) : this(objectToTrack.gameObject){}
		public override void Update(){
			base.Update();
			if (CameraController.main.debugcube != null){
				CameraController.main.debugcube.transform.position = objectPredictedPos;
			}
		}
	}

	public class FollowShot : Shot{
		protected GameObject followObject;
		protected Vector3 cameraOffset;
		protected Vector3 trackingOffset;
		protected Vector3 globalTrackingOffset;
		protected Vector3 trackLoc{
			get{return ObjectToTrack.transform.TransformPoint(trackingOffset);}
		}
		protected bool inheritRotation = true;
		float smoothTimeFactor;

		public FollowShot(GameObject followAndTrack, Vector3 localCameraOffset, float smoothTimeFactor = 1f)
		:this(followAndTrack, localCameraOffset, Vector3.zero, smoothTimeFactor){}

		public FollowShot(GameObject followAndTrack, Vector3 localCameraOffset, Vector3 localTrackingOffset, float smoothTimeFactor = 1f)
		:this(followAndTrack, localCameraOffset, followAndTrack, localTrackingOffset, smoothTimeFactor){}

		public FollowShot(GameObject followObject, Vector3 localCameraOffset, GameObject objectToTrack, Vector3 localTrackingOffset, float smoothTimeFactor = 1)
		:this(followObject, localCameraOffset, Vector3.zero, objectToTrack, localTrackingOffset, smoothTimeFactor){}

		public FollowShot(GameObject followObject, Vector3 localCameraOffset, Vector3 worldCameraOffset, GameObject objectToTrack, Vector3 localTrackingOffset, float smoothTimeFactor = 1) : base(){
			this.followObject = followObject;
			this.ObjectToTrack = objectToTrack;
			this.cameraOffset = localCameraOffset;
			this.globalTrackingOffset = worldCameraOffset;
			base.trackingOffset = localTrackingOffset;
			this.trackingOffset = localTrackingOffset;
			this.smoothTimeFactor = smoothTimeFactor;
		}

		public override void Update(){
			base.Update();
			CameraPosition = followObject.transform.TransformPoint(cameraOffset) + globalTrackingOffset;
			Vector3 forward = trackLoc - CameraPosition;
			CameraForward = forward;
//			this.SmoothSpeed = smoothTimeFactor/(Mathf.Pow(Vector3.Distance(CameraController.main.transform.position, CameraPosition), 1.5f))* (transitionTime / smoothspeedmultiplier)* (transitionTime / smoothspeedmultiplier);
//			if (Vector3.Distance(Camera.main.transform.position, CameraPosition) < 5f){
//				Debug.Log("reducing smoothspeed " + SmoothSpeed);
//				SmoothSpeed = Mathf.Clamp(SmoothSpeed - Time.deltaTime, 0, 20f);
//			}
		}
	}

	public class FollowShotNoRotation : FollowShot{
		private bool initialized = false;
		public FollowShotNoRotation(GameObject objectToTrack) : base(objectToTrack, Vector3.zero, Vector3.zero){
			initialized = false;
		}
		public FollowShotNoRotation(GameObject objectToTrack, Vector3 worldCameraOffset, Vector3 localTrackingOffset) : base(objectToTrack, worldCameraOffset, localTrackingOffset){
			initialized = true;
		}

		public override void Update(){
			base.Update();
			if (!initialized){
				cameraOffset = Camera.main.transform.position - objectPredictedPos;
				initialized = true;
			}
			CameraPosition = objectPredictedPos + cameraOffset;
			Vector3 forward = trackLoc - CameraPosition;
			CameraForward = forward;
		}
	}


	public class TrackingShot : Shot{
		private Vector3 cameraOffset;
		private Vector3 trackingOffset;
		private Vector3 trackLoc{
			get{return ObjectToTrack.transform.TransformPoint(trackingOffset) + objectVelocity;}
		}

		private Vector3 targetCameraPosition;
		private bool initialized= false;
		private Vector3 velocity = Vector3.zero;
		private float positionSmoothtime;
		private GameObject offsetObject;
		public TrackingShot(GameObject objectToTrack, Vector3 worldCameraOffset, float smoothtime = 1f) : this(objectToTrack, Camera.main.gameObject, worldCameraOffset, smoothtime, Vector3.zero){}
		public TrackingShot(GameObject objectToTrack, GameObject offsetObject, Vector3 worldCameraOffset, float smoothtime) : this(objectToTrack, offsetObject, worldCameraOffset, smoothtime, Vector3.zero){}
		public TrackingShot(GameObject objectToTrack, GameObject offsetObject, Vector3 worldCameraOffset, float smoothtime, Vector3 localTrackingOffset) : base(){
			this.ObjectToTrack = objectToTrack;
			this.cameraOffset = worldCameraOffset;
			this.trackingOffset = localTrackingOffset;
			base.trackingOffset = this.trackingOffset;
			this.positionSmoothtime = smoothtime;
			this.offsetObject = offsetObject;
		}
		public override void Update(){
			base.Update();
			if (!initialized){
				targetCameraPosition = offsetObject.transform.position + cameraOffset;
				CameraPosition = Camera.main.transform.position;
				initialized = true;
			}
			CameraPosition = Vector3.SmoothDamp(CameraPosition, targetCameraPosition, ref velocity, positionSmoothtime);
			Vector3 forward = trackLoc - CameraPosition;
			//Debug.DrawRay(CameraPosition, forward*10f, Color.yellow);
			CameraForward = forward;
		}

	}



//	//camera position follows a gameobject with specified offset, always.
//	public class FollowShot : Shot{
//		private Vector3 followPositionOffset;
//		private Vector3 trackPositionOffset;
//		public FollowShot(GameObject objectToFollow, Vector3 followOffset, Vector3 trackOffset)
//			: base(objectToFollow.transform.position, objectToFollow.transform.forward){
//			this.ObjectToTrack = objectToFollow;
//			this.followPositionOffset = followOffset;
//			this.trackPositionOffset = trackOffset;
//			this.SmoothSpeed = .01f;
//		}
//
//		public FollowShot(GameObject objectToFollow, Vector3 followOffset)
//			: this(objectToFollow, followOffset, Vector3.zero){}
//
//		public override void Update(){
////			Vector3 localOffset = ObjectToTrack.transform.TransformPoint(followPositionOffset);
////			Vector3 localTrackOffset = ObjectToTrack.transform.TransformPoint(trackPositionOffset);
//			CameraPosition = followPositionOffset;
//
////			CameraPosition = ObjectToTrack.transform.position + followPositionOffset;
////			Camera.main.transform.position = localOffset;
//			Camera.main.transform.LookAt(trackPositionOffset);
////			Forward = (localTrackOffset - Camera.main.transform.position);
//
//		}
//	}






}
