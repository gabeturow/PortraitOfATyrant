using UnityEngine;
using System.Collections;

public class TurnOffThroughAnimation : MonoBehaviour {

	public string changeThisVariable;
	public bool changeVariableToThis;

	public string changeThisVariable1;
	public bool changeVariableToThis1;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if(changeThisVariable!=null){
			GameMan.main.conditionals.SetValue(changeThisVariable,changeVariableToThis);
		}
		if(changeThisVariable1!=null){
			GameMan.main.conditionals.SetValue(changeThisVariable1,changeVariableToThis1);
		}
	}
}
