using UnityEngine;
using System.Collections;

public class CharacterBrain : MonoBehaviour {

	public CharacterMotor motor{get; private set;}
	public CharacterRenderer render{get; private set;}

	// Use this for initialization
	void Start () {
		motor = GetComponentInChildren<CharacterMotor>();
		render = GetComponentInChildren<CharacterRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		render.SetVelocity(motor.currentVelocity);
	}
}
