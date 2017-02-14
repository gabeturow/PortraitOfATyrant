using UnityEngine;
using System.Collections;

public class RoomTransitionTester : MonoBehaviour {

	[SerializeField]
	Room r;

	[SerializeField]
	string beginDoor;

	// Use this for initialization
	void Start () {
		RoomMan.main.LoadRoom(r.gameObject.name, beginDoor);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
