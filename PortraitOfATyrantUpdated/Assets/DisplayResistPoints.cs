using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class DisplayResistPoints : MonoBehaviour {
	public Text thisTextObject;
	void Update () {

		thisTextObject.text="RESISTANCE\n "+PointsController.resistancePoints+"/100";;
	}
}

