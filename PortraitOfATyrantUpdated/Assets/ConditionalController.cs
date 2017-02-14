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
	public GameObject cookWhistling;
	public GameObject startMenu;

	DialogueViewer dialogueViewer;
	// Use this for initialization
	void Awake(){
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
		GameMan.main.conditionals.SetValue("TALKEDSMUGGLER", false);
		GameMan.main.conditionals.SetValue("TALKEDWHALER", false);
		GameMan.main.conditionals.SetValue("WHALERBEGIN", false);
		GameMan.main.conditionals.SetValue("GRIEVANCE", false);
		GameMan.main.conditionals.SetValue("RIDDLE", false);
		GameMan.main.conditionals.SetValue("DIALOGUE", false);
		GameMan.main.conditionals.SetValue("FREEDSMUGGLER", false);
		GameMan.main.conditionals.SetValue("FREEDWHALER",false);
		GameMan.main.conditionals.SetValue("WHALERUNCHAINED", false);
		GameMan.main.conditionals.SetValue("SHIPBURNING", false);
		GameMan.main.conditionals.SetValue("RIGHTS", false);
		GameMan.main.conditionals.SetValue("CONFLICT", false);
		GameMan.main.conditionals.SetValue("NIGHTTIME", false);
		GameMan.main.conditionals.SetValue("COOKWHISTLEEND", false);
		GameMan.main.conditionals.SetValue("LOOKOUTTALKED", false);
		GameMan.main.conditionals.SetValue("AMERICANTALKED", false);
		GameMan.main.conditionals.SetValue("BETWEENNIGHTANDLOOKOUT", false);
		GameMan.main.conditionals.SetValue("HOLDUNLOCKED", false);
		GameMan.main.conditionals.SetValue("BREADUNLOCKED", false);
		GameMan.main.conditionals.SetValue("MAGAZINEUNLOCKED", false);
		GameMan.main.conditionals.SetValue("COOKFINISHED", false);
		GameMan.main.conditionals.SetValue("MENURETURN", false);


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

		}else{
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
			cookWhistling.SetActive(true);
			upperDeckTheme.enabled=false;
		}else if (GameMan.main.conditionals.GetValue("UPPERDECK")){
			upperDeckTheme.enabled=true;
			cookWhistling.SetActive(false);
			//conflictTheme.enabled=false;
			belowDeckTheme.enabled=false;
		}else{
			upperDeckTheme.enabled=false;
			belowDeckTheme.enabled=false;
		}



		if (GameMan.main.conditionals.GetValue("GRIEVANCE")){

			belowDeckTheme.enabled=false;
			conflictTheme.enabled=true;
			upperDeckTheme.enabled=false;
		}else if(GameMan.main.conditionals.GetValue("CONFLICT")){
				conflictTheme.enabled=true;
				belowDeckTheme.enabled=false;
				upperDeckTheme.enabled=false;
		}else{
			conflictTheme.enabled=false;
		}


		/*if(GameMan.main.conditionals.GetValue("STARTMENU")){
			startMenu.GetComponent<CanvasGroupFader>().displaying=true;
		}*/

		//CONTROLS WHALER SINGING
		if(GameMan.main.conditionals.GetValue("WHALERSINGING")){
			WhalerSinging.enabled=true;
		}else{
			WhalerSinging.enabled=false;
		}

		if(GameMan.main.conditionals.GetValue("TALKED_ALREADY")){
			BreadRoomSounds.SetActive(false);
		}

		if(GameMan.main.conditionals.GetValue("COOKWHISTLEEND")){
			cookWhistling.SetActive(false);
		}else if(GameMan.main.conditionals.GetValue("COOKWHISTLEEND") && GameMan.main.conditionals.GetValue("UPPERDECK")==false ){
			cookWhistling.SetActive(true);
		}
	}
}