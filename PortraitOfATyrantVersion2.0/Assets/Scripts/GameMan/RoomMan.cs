using UnityEngine;
using System.Collections;

public class RoomMan : MonoBehaviour {
	public static RoomMan main;

	public Room current;


	void Awake(){
		main = this;
	}



	public void LoadRoom(Room r, string doorwayName){
		//hide toast
		Toaster.main.Hide();

		//do loading
		LoadingScreenController.main.FadeToBlack(()=>{
			TapInteractor.main.enabled = false;
			if (current != null){
				Destroy(current.gameObject);
			}
			GameObject newRoom = Instantiate(r.gameObject) as GameObject;
			newRoom.transform.position = Vector3.zero;
			current = newRoom.GetComponent<Room>();
			Transform entryway = newRoom.transform.FindDeepChild(doorwayName);
			if (entryway != null){
				var pos = GameMan.main.character.transform.position;
				pos.x = entryway.position.x;
				pos.z = entryway.position.z;
				GameMan.main.character.transform.position = pos;
				GameMan.main.character.motor.target = pos;
				GameMan.main.character.motor.currentHeight = newRoom.transform.position.y + r.FloorHeight;

				//set camera to room static or not
				CameraController2D.main.isActive = !r.staticCamera;
				CameraController2D.main.useMinMax = true;
				var rPos = new Vector2(newRoom.transform.position.x, newRoom.transform.position.y);
				CameraController2D.main.min = rPos + r.CameraMin;
				CameraController2D.main.max = rPos + r.CameraMax;
			}
		},
			()=>{
				TapInteractor.main.enabled = true;
			}
		);

	}

}
