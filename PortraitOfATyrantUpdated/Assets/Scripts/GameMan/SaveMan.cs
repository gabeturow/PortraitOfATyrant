using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class SaveMan {

	const string saveFileNameDefault = "portrait.sav";


	public static SaveFile SaveToFile(string fileName = saveFileNameDefault){
		var saveDirectory = Application.persistentDataPath;
		var fullPath = Path.Combine(saveDirectory, fileName);
		var saveFile = CreateSaveFile();
		var saveFileJson = JsonUtility.ToJson(saveFile);
		File.WriteAllText(fullPath, saveFileJson);
		return saveFile;
	}

	/// <summary>
	/// Reads a savefile from disk. Returns a savefile if it worked
	/// </summary>
	public static SaveFile TryLoad(string fileName = saveFileNameDefault){
		var saveDirectory = Application.persistentDataPath;
		var fullPath = Path.Combine(saveDirectory, fileName);

		var exists = File.Exists(fullPath);
		if (!exists) {
			Debug.Log("No save file to load");
			return null;
		}

		var fileText = File.ReadAllText(fullPath);

		var saveFile = JsonUtility.FromJson<SaveFile>(fileText);
		if (saveFile == null) {
			Debug.Log("Save file cannot be read!");
			return null;
		}

		return saveFile;
	}

	/// <summary>
	/// Restores the data from a SaveFile into the game
	/// </summary>
	public static bool RestoreSave(SaveFile file){
		try{
			//room gets restored by gameman.

			//restore conditionals
			GameMan.main.conditionals.FromList(file.conditonals);

			//restore inventory
			for(int i = 0; i < file.inventory.Length; i++){
				var inventoryObject = InventoryLibrary.GetObject(file.inventory[i].key);
				if (inventoryObject != null){
					InventoryMan.main.Add(inventoryObject);
				}
			}

			return true;
		}
		catch(System.Exception e){
			Debug.Log(e);
		}
		return false;
	}

	public static SaveFile CreateSaveFile(){
		var saveFile = new SaveFile();
		//save player
		saveFile.player.roomName = RoomMan.main.lastRoom;
		saveFile.player.doorName = RoomMan.main.lastDoor;

		//save conditionals
		saveFile.conditonals = GameMan.main.conditionals.ToList();

		//save inventory
		var items = InventoryMan.main.GetItems();
		var itemList = new SaveFile.InventoryObject[items.Length];
		for (int i = 0; i < items.Length; i++) {
			itemList[i] = new SaveFile.InventoryObject {
				key = items[i].name
			};
		}

		saveFile.inventory = itemList;

		return saveFile;
	}


}

[Serializable]
public class SaveFile{

	public Player player = new Player();

	[Serializable]
	public class Player{
		public string roomName;
		public string doorName;
	}

	public Conditional[] conditonals = new Conditional[0];

	[Serializable]
	public class Conditional{
		public string key;
		public string value;
	}

	public InventoryObject[] inventory = new InventoryObject[0];

	[Serializable]
	public class InventoryObject{
		public string key;
	}
}
