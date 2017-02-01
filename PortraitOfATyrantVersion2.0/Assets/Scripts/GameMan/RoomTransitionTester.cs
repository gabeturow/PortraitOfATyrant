using UnityEngine;
using System.Collections;

public class RoomTransitionTester : MonoBehaviour {

	[SerializeField]
	Room r;

	[SerializeField]
	string beginDoor;

	// Use this for initialization
	void Start () {
		RoomMan.main.LoadRoom(r, beginDoor);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
