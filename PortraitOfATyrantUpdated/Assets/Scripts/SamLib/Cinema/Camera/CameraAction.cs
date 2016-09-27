using UnityEngine;
using System.Collections;

namespace SamCinema{
	public class CameraAction : SamAction {
		public Shot shot{get; set;}

		public CameraAction(Shot shot, float seconds = 0) : base(){
			this.shot = shot;
			this.OnStart = ()=>{CameraController.main.currentShot = shot;};
			this.Length = seconds;
		}
	}

	/// <summary>
	/// Camera zoom action.
	/// Changes eiher ortho size or fov of the main camera.
	/// </summary>
	public class CameraZoomAction : SamAction{
		public CameraZoomAction(float fov, float speed, float seconds = 0) : base(){
			this.OnStart = ()=>CameraController.main.SetFov(fov, speed);
			this.Length = seconds;
		}
		public CameraZoomAction(float fov) : base(){
			this.OnStart = ()=>CameraController.main.SetFov(fov);
		}
	}


	/// <summary>
	/// Changes the camera composition. The Vector2 you pass in is the new "Center" of the screen.
	/// Vector2.zero is the true center of the screen.
	/// (0, 1) is Upper Center.
	/// (0, -1) is Lower Center.
	/// (-1, 0) is Left Center.
	/// Etc.
	/// </summary>
	public class CameraComposition: SamAction{
		public CameraComposition() : this(Vector2.zero){}
		public CameraComposition(Vector2 screenFocusCenterPos) : base(){
			this.OnStart = ()=>{
				CameraController.main.compositionTranslation = Vector3.ClampMagnitude(screenFocusCenterPos, 1f);
			};
		}
	}


}
