using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMan : MonoBehaviour {
	public static GameMan main;
	public CharacterBrain character;

	ConditionalMan _conditionals;
	public ConditionalMan conditionals{
		get { 
			if (_conditionals == null){
				_conditionals = new ConditionalMan();
			}
			return _conditionals;
		}
	}

	void Awake(){
		main = this;

	}

	// Use this for initialization
	void Start () {
		TapInteractor.main.characterToMove = character.motor;
		CameraController2D.main.Track(character.gameObject);

		var saveFile = TryLoadSave() ?? GetNewSaveFile();
		RoomMan.main.LoadRoom(saveFile.player.roomName, saveFile.player.doorName);
	}


	SaveFile TryLoadSave(){
		var saveFile = SaveMan.TryLoad();
		if (saveFile == null) return null;
		var restored = SaveMan.RestoreSave(saveFile);
		if (!restored) return null;
		return saveFile;
	}


	SaveFile GetNewSaveFile(){
		var saveFile = SaveMan.CreateSaveFile();
		saveFile.player.roomName = "CaptainsQuarters";
		saveFile.player.doorName = "RightCaptainDoor";
		return saveFile;
	}


}
