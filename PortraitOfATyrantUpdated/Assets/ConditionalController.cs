using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConditionalController : MonoBehaviour {
	public static ConditionalController main;
	public GameObject BreadRoomSounds;
	//public AudioSource backgroundMusic;
	public AudioSource belowDeckTheme;
	public AudioSource upperDeckTheme;
	public AudioSource conflictTheme;
	public AudioSource WhalerSinging;
	public GameObject rightsPanel;
	public GameObject dialoguePanel;
	public GameObject grievancePanel;
	public GameObject prompt;
	public GameObject riddlePrompt;
	DialogueViewer dialogueViewer;
	// Use this for initialization
	void Start(){
		//Variables for Chapter 1
		GameMan.main.conditionals.SetValue("BREADROOMSOUNDS", false);
		GameMan.main.conditionals.SetValue("DOORLEFTUSED", false);
		GameMan.main.conditionals.SetValue("UPPERDECK", false);
		GameMan.main.conditionals.SetValue("BELOWDECK", true);
		GameMan.main.conditionals.SetValue("COOKFINISHED", false);
		GameMan.main.conditionals.SetValue("WHALERSINGING", true);
		GameMan.main.conditionals.SetValue("LAMPON", false);
		GameMan.main.conditionals.SetValue("FAN_GIVEN", false);
		GameMan.main.conditionals.SetValue("CHESTOPEN", false);
		GameMan.main.conditionals.SetValue("DRAWEROPEN", false);
		GameMan.main.conditionals.SetValue("DUDINGSTONPOINTS", false);
		GameMan.main.conditionals.SetValue("SHIPBURNING", true);
		GameMan.main.conditionals.SetValue("GRIEVANCE", false);
		GameMan.main.conditionals.SetValue("RIDDLE", false);
		GameMan.main.conditionals.SetValue("DIALOGUE", false);
		GameMan.main.conditionals.SetValue("FREEDSMUGGLER", false);
		GameMan.main.conditionals.SetValue("FREEDWHALER",false);
		GameMan.main.conditionals.SetValue("RIGHTS", false);
		GameMan.main.conditionals.SetValue("CONFLICT", false);
		GameMan.main.conditionals.SetValue("NIGHTTIME", true);

		BreadRoomSounds.GetComponent<AudioSource>().Play();
	}
	void Update(){

		//Turn on Dialogue Interfaces
		if(GameMan.main.conditionals.GetValue("RIGHTS")){	
			GameMan.main.conditionals.SetValue("GRIEVANCE", false);
			GameMan.main.conditionals.SetValue("RIDDLE", false);
			GameMan.main.conditionals.SetValue("DIALOGUE", false);
			prompt.SetActive(false);
			dialoguePanel.SetActive(false);
			grievancePanel.SetActive(false);
			rightsPanel.SetActive(true);

		}else if(GameMan.main.conditionals.GetValue("GRIEVANCE")){			
			dialoguePanel.SetActive(false);
			grievancePanel.SetActive(true);
			rightsPanel.SetActive(false);
			prompt.SetActive(true);
			GameMan.main.conditionals.SetValue("DIALOGUE", false);
			GameMan.main.conditionals.SetValue("RIDDLE", false);
			GameMan.main.conditionals.SetValue("RIGHTS", false);
			//prompt.GetComponent<Text>.text=
		}else if(GameMan.main.conditionals.GetValue("RIDDLE")){
			GameMan.main.conditionals.SetValue("GRIEVANCE", false);
			GameMan.main.conditionals.SetValue("DIALOGUE", false);
			GameMan.main.conditionals.SetValue("RIGHTS", false);
			prompt.SetActive(false);
			dialoguePanel.SetActive(true);
			grievancePanel.SetActive(false);
			rightsPanel.SetActive(false);

		}else if(GameMan.main.conditionals.GetValue("DIALOGUE")){	
			GameMan.main.conditionals.SetValue("GRIEVANCE", false);
			GameMan.main.conditionals.SetValue("RIDDLE", false);
			GameMan.main.conditionals.SetValue("RIGHTS", false);

			prompt.SetActive(false);
			dialoguePanel.SetActive(true);
			grievancePanel.SetActive(false);
			rightsPanel.SetActive(false);

		}
				
		
		//THIS CONTROLS THE BREADROOM SOUNDS THAT COVER THE CAPTAINS QUARTERS AND THE MAGAZINE BUT TURN OFF FOR THE HOLD
		if(GameMan.main.conditionals.GetValue("BREADROOMSOUNDS")){
			BreadRoomSounds.SetActive(true);
		}else{
			BreadRoomSounds.SetActive(false);
		}

	
		//THIS CONTROLS THE THEME MUSIC

		if(GameMan.main.conditionals.GetValue("BELOWDECK")){// && !GameMan.main.conditionals.GetValue("GRIEVANCE")){
			belowDeckTheme.enabled=true;
			//conflictTheme.enabled=false;
			upperDeckTheme.enabled=false;
		}else if (GameMan.main.conditionals.GetValue("UPPERDECK")){
			upperDeckTheme.enabled=true;
			//conflictTheme.enabled=false;
			belowDeckTheme.enabled=false;
		}

		if(GameMan.main.conditionals.GetValue("CONFLICT")){
				conflictTheme.enabled=true;
				belowDeckTheme.enabled=false;
				upperDeckTheme.enabled=false;
			}
		if (GameMan.main.conditionals.GetValue("GRIEVANCE")){

			belowDeckTheme.enabled=false;
			conflictTheme.enabled=true;
			upperDeckTheme.enabled=false;
		}



		//CONTROLS WHALER SINGING
		if(GameMan.main.conditionals.GetValue("WHALERSINGING")){
			WhalerSinging.enabled=true;
		}else{
			WhalerSinging.enabled=false;
		}
	}
}