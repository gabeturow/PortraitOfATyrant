using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//use this to load everything into memory first
//so we don't have to use Resources.Load
//uses a dictionary to sort all the prefabs so you can get by string

public class PrefabManager : MonoBehaviour {
	private List<GameObject> scannedPrefabs;

	private static Dictionary<string, GameObject> _prefabDict;

	private static Dictionary<string, GameObject> prefabDict{
		get{
			if (_prefabDict == null){
				LoadAllPrefabs();
			}
			return _prefabDict;
		}
	}

	private static string path = "Prefabs";

	private static Queue<Object> timeDestroy = new Queue<Object>();
	private float destroyerTimer = 1f;

	public static void AddDestroyQueue(GameObject g){
		g.SetActive(false);
		timeDestroy.Enqueue(g);
	}
	public static void AddDestroyQueue(Object o){
		timeDestroy.Enqueue(o);
	}

	void LateUpdate(){
		if (destroyerTimer > 0){
			destroyerTimer -= Time.unscaledDeltaTime;
			return;
		}
		if (timeDestroy.Count > 0){
			var o = timeDestroy.Dequeue();
			if (o != null){
				Destroy(o);
			}
		}
		destroyerTimer = 1f;

	}

	//make those prefabs into the dictionary
	//run automatically.
	//make sure this runs before instantiating any prefabs using this script.
	void Awake(){
		LoadAllPrefabs();
	}


	//use this to get a prefab
	public static GameObject GetPrefab(string prefabName){
		return prefabDict[prefabName];
	}

	//instantiate and get component
	public static T Instantiate<T>(string prefabName) where T : MonoBehaviour{
		var ans = Instantiate(prefabName).GetComponent<T>();
		if (ans == null){
			Debug.LogError("Prefab doesn't have component " + typeof(T).ToString());
		}
		return ans;
	}

	//instantiate objects with a string
	public static GameObject Instantiate(string prefabName){
		GameObject instance = Instantiate(prefabName, Vector3.zero, Quaternion.identity);
		return instance;
	}

	public static GameObject Instantiate(string prefabName, Vector3 location){
		GameObject instance = Instantiate(prefabName, location, Quaternion.identity);
		return instance;
	}

	public static GameObject Instantiate(string prefabName, Vector3 location, Quaternion rotation){
		GameObject prefab = GetPrefab(prefabName);
		if (prefab == null) {
			Debug.LogError("Cannot load prefab " + prefabName);
			return null;
		}
		GameObject instance = Instantiate(prefab, location, rotation) as GameObject;
		return instance;
	}


	//this is run automatically.
	static void LoadAllPrefabs(){
		GameObject[] prefabs = Resources.LoadAll<GameObject>(path);
		_prefabDict = new Dictionary<string, GameObject>();
		for(int i = 0; i < prefabs.Length; i++){
			_prefabDict.Add (prefabs[i].name, prefabs[i]);
		}
	}




}
