using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointsController : MonoBehaviour {
	[SerializeField]
	public int points=0;
	[SerializeField]
	public static int resistancePoints=50;

	public static PointsController main;

	public Text DisplayTextObject;

	public Text DisplayTextObjectResist;
	// Use this for initialization
	void Start () {
		main=this;
	}
	
	// Update is called once per frame
	void Update () {

		DisplayTextObject.text="PERSUASION\n<color=#ffff00ff><i>NOVICE</i></color>\n "+points+"/400";
		DisplayTextObjectResist.text="RESISTANCE\n "+resistancePoints+"/100";
	}
}
