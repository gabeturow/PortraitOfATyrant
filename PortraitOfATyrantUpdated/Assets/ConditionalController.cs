using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConditionalController : MonoBehaviour {
	public GameObject BreadRoomSounds;
	public AudioSource ThemeMusicBelowDeck;
	public AudioSource ThemeMusicAboveDeck;
	public GameObject WhalerSinging;
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
		GameMan.main.conditionals.SetValue("COOKFINISHED", false);
		GameMan.main.conditionals.SetValue("WHALERSINGING", true);
		GameMan.main.conditionals.SetValue("LAMPON", false);
		GameMan.main.conditionals.SetValue("FAN_GIVEN", false);
		GameMan.main.conditionals.SetValue("CHESTOPEN", false);
		GameMan.main.conditionals.SetValue("DRAWEROPEN", false);
		GameMan.main.conditionals.SetValue("DUDINGSTONPOINTS", false);
		GameMan.main.conditionals.SetValue("SHIPBURNING", false);
		GameMan.main.conditionals.SetValue("GRIEVANCE", false);
		GameMan.main.conditionals.SetValue("RIDDLE", false);
		GameMan.main.conditionals.SetValue("DIALOGUE", false);


		BreadRoomSounds.GetComponent<AudioSource>().Play();
	}
	void Update(){

		//Turn on Dialogue Interfaces

		if(GameMan.main.conditionals.GetValue("GRIEVANCE")){			
			dialoguePanel.SetActive(false);
			grievancePanel.SetActive(true);
			prompt.SetActive(true);
			GameMan.main.conditionals.SetValue("DIALOGUE", false);
			GameMan.main.conditionals.SetValue("RIDDLE", false);
			//prompt.GetComponent<Text>.text=
		}else{
			prompt.SetActive(false);
			grievancePanel.SetActive(false);
		}
		if(GameMan.main.conditionals.GetValue("RIDDLE")){
			GameMan.main.conditionals.SetValue("GRIEVANCE", false);
			GameMan.main.conditionals.SetValue("DIALOGUE", false);
			prompt.SetActive(false);
			dialoguePanel.SetActive(true);
			grievancePanel.SetActive(false);
		}else{
			riddlePrompt.GetComponent<Image>().enabled=false;
		}

		if(GameMan.main.conditionals.GetValue("DIALOGUE")){	
			GameMan.main.conditionals.SetValue("GRIEVANCE", false);
			GameMan.main.conditionals.SetValue("RIDDLE", false);
			prompt.SetActive(false);
			dialoguePanel.SetActive(true);
			grievancePanel.SetActive(false);
		}
				
		
		//THIS CONTROLS THE BREADROOM SOUNDS THAT COVER THE CAPTAINS QUARTERS AND THE MAGAZINE BUT TURN OFF FOR THE HOLD
		if(GameMan.main.conditionals.GetValue("BREADROOMSOUNDS")){
			BreadRoomSounds.SetActive(true);
		}else{
			BreadRoomSounds.SetActive(false);
		}

	
		//THIS CONTROLS THE THEME MUSIC
		
		if(!GameMan.main.conditionals.GetValue("UPPERDECK")){
			ThemeMusicBelowDeck.enabled=true;
			ThemeMusicAboveDeck.enabled=false;
		}else{
			ThemeMusicAboveDeck.enabled=true;
			ThemeMusicBelowDeck.enabled=false;
		}

		//CONTROLS WHALER SINGING
		if(GameMan.main.conditionals.GetValue("WHALERSINGING")){
			WhalerSinging.SetActive(true);
		}else{
			WhalerSinging.SetActive(false);
		}
	}
}