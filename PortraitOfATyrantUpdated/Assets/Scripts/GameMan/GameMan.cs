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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
