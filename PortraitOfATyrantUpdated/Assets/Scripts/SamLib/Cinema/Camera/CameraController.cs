using UnityEngine;
using System.Collections;


namespace SamCinema{
	//attach this to the main camera
	public class CameraController : MonoBehaviour {
		public static CameraController main;
		public Camera camera;
		public GameObject debugcube;
		public bool orthographic{get{return Camera.main.orthographic;} set{Camera.main.orthographic = value;}}

		private Vector3Damper positionDamper;
		private FloatDamper fovDamper;

		private Shot _currentShot;
		public Shot currentShot{get{return _currentShot;} set{_currentShot = value;}}

		public bool limitRoll;


		public void SetFov(float fov){SetFov(fov, fovDamper.Speed);}
		public void SetFov(float fov, float speed){
			fovDamper.Target = fov;
			if (speed > 0){
				fovDamper.Speed = speed;
			}
			else{
				fovDamper.Value = fov;
			}
		}

		public Vector2 compositionTranslation = Vector2.zero;

		public float smoothTime = .5f;

		public GameObject FocusObject{
			get{return currentShot == null ? null : currentShot.ObjectToTrack == null ? null : currentShot.ObjectToTrack;}
		}

		public float FocusDist{
			get{return FocusObject == null ? 10f : Vector3.Distance(transform.position, FocusObject.transform.position);}
		}

		public Vector3 FocusTarget{
			get{return currentShot.objectPredictedPos;}
		}

		/// <summary>
		/// The higher the exp, the more SmoothSpeed will affect rotation.
		/// </summary>
		public float rotationSmoothSpeedExpStrength = 1.5f;


		/// <summary>
		/// use angle power smoothing? This makes the camera go even faster, if the angle is big.
		/// </summary>
		public bool useAnglePowerSmoothing = false;

		/// <summary>
		/// The angle power smoothing. Above this angle, the camera will go faster. Below this angle, the camera will move slower.
		/// </summary>
		public float anglePowerSmoothing = 40f;

		/// <summary>
		/// The power of the angle smoothing effect.
		/// </summary>
		public float anglePowerExpStrength = 2f;

		void Awake(){
			if (main == null)
			main = this;
			if (this.camera == null){
				this.camera = GetComponent<Camera>();
			}
		}

		void Start(){
			positionDamper = new Vector3Damper(transform.position, .5f);
			SmoothDamper.main.RemoveDamper(positionDamper);
			fovDamper = new FloatDamper(orthographic ? camera.orthographicSize : camera.fieldOfView, 2f);
		}


		void LateUpdate () {
			if (currentShot != null){
				CalculateShot();
				SetShotValues();
			}
			if (camera.orthographic){
				camera.orthographicSize = Mathf.Clamp(fovDamper.Value,0.01f,1000f); //20
			}
			else 
				camera.fieldOfView = fovDamper.Value;

			if (limitRoll){
				var rot = transform.localRotation.eulerAngles;
				rot.z = 0;
				transform.localRotation = Quaternion.Euler(rot);
			}

		}

		void CalculateShot(){
			currentShot.Update();
			positionDamper.Speed = currentShot.SmoothSpeed;
			positionDamper.Target = currentShot.CameraPosition;
		}

		Quaternion GetCompositionRotationPersp(){
			float x = compositionTranslation.x;
			float y = compositionTranslation.y;
			float fv = camera.fieldOfView;
			float fh = VerticalToHorizontalFov(fv)/2f;
			fv /= 2f;
			return Quaternion.Euler(fv * y, fh * -x, 0f);
		}

		float VerticalToHorizontalFov(float vfov){
			float radFov = vfov * Mathf.Deg2Rad;
			return 2 * Mathf.Atan(Mathf.Tan(radFov/2f) * camera.aspect) * Mathf.Rad2Deg;
		}

		Vector3 GetCompositionOffset(){
			Vector3 compTrans = compositionTranslation * (orthographic ? 1f : .1f);
			Vector3 delta = FocusTarget - transform.position;
			Vector3 deltaRight = Vector3.Cross(delta, Vector3.up);
			Vector3 deltaUp = Vector3.Cross(deltaRight, delta);
			Vector3 compositionOffset = (deltaRight.normalized * compTrans.x + deltaUp.normalized * compTrans.y);
			float scalar = 0f;

			return compositionOffset * scalar * -1;
		}

		void SetShotValues(){
			positionDamper.UpdateDeltaTime();
			if (currentShot.SmoothSpeed <= .01f) {
				transform.position = currentShot.CameraPosition;
			}
			else{
				transform.position = positionDamper.Value; 
			}


			Vector3 lookDelta = (FocusObject != null) ? ((FocusTarget + GetCompositionOffset ()) - transform.position).normalized :
														(currentShot.CameraForward.normalized);
			Quaternion targetRotation = Quaternion.LookRotation (lookDelta, Vector3.up);
			if (!camera.orthographic) {
				targetRotation *= GetCompositionRotationPersp ();
			}
			float angle = Quaternion.Angle (targetRotation, transform.rotation);
			float t = Time.deltaTime * Mathf.Pow ((1 / currentShot.SmoothSpeed), rotationSmoothSpeedExpStrength) * 2.0f;
			if (useAnglePowerSmoothing)	t *= Mathf.Pow ((angle / anglePowerSmoothing), anglePowerExpStrength);

			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, t);

		}


		void OnDestroy(){
			SmoothDamper.main.RemoveDamper(positionDamper, fovDamper);
		}
	}
}
