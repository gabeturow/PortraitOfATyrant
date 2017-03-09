using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConditionalController : MonoBehaviour {
	public static ConditionalController main;
	//public AudioSource backgroundMusic;
	public AudioSource belowDeckTheme;
	public GameObject rightsPanel;
	public GameObject dialoguePanel;
	public GameObject grievancePanel;
	public GameObject prompt;
	public GameObject riddlePrompt;
	public GameObject startMenu;
	public GameObject OldManTrans;
	public GameObject turnOnBurningScene;
	public GameObject endLevelOneCanvas;
	public GameObject startButton;
	public GameObject endCredits;
	public GameObject startOver;
	public GameObject BreadRoomExit;
	public GameObject BreadRoomExitFromMagazine;

	public GameObject score;
	public GameObject homeButton;

	public AudioClip belowDeckThemeClip;
	public AudioClip upperDeckClip;
	public AudioClip conflictClip;


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
		GameMan.main.conditionals.SetValue("TALKEDSMUGGLER", false);
		GameMan.main.conditionals.SetValue("TALKEDWHALER", false);
		GameMan.main.conditionals.SetValue("WHALERBEGIN", false);
		GameMan.main.conditionals.SetValue("GRIEVANCE", false);
		GameMan.main.conditionals.SetValue("RIDDLE", false);
		GameMan.main.conditionals.SetValue("DIALOGUE", false);
		GameMan.main.conditionals.SetValue("FREEDSMUGGLER", false);
		GameMan.main.conditionals.SetValue("FREEDWHALER",false);
		GameMan.main.conditionals.SetValue("WHALERUNCHAINED", false);
		GameMan.main.conditionals.SetValue("SHIPBURNING", true);
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
		GameMan.main.conditionals.SetValue("OLDMAN", false);
		GameMan.main.conditionals.SetValue("STARTGAME", true);
		GameMan.main.conditionals.SetValue("STARTGAMEFINISHED", false);
		GameMan.main.conditionals.SetValue("TALKEDAMERICAN", false);
		GameMan.main.conditionals.SetValue("FINISHEDAMERICAN", false);
		GameMan.main.conditionals.SetValue("TALKEDCOOK", false);
		GameMan.main.conditionals.SetValue("FINISHEDCOOK", false);
		GameMan.main.conditionals.SetValue("TALKEDSMUGGLER", false);
		GameMan.main.conditionals.SetValue("FINISHEDSMUGGLER", false);
		GameMan.main.conditionals.SetValue("CUTSCENERUNNING", true);
		GameMan.main.conditionals.SetValue("BURNINGSCENE",false);
		GameMan.main.conditionals.SetValue("ENDGAME",false);
		GameMan.main.conditionals.SetValue("GAMEOVER",false);
		GameMan.main.conditionals.SetValue("MAGAZINE",false);


		}
	AudioClip currentSongState;

	public void ChangeTheMusic(){

		if (GameMan.main.conditionals.GetValue("GRIEVANCE") || GameMan.main.conditionals.GetValue("RIGHTS") || GameMan.main.conditionals.GetValue("CONFLICT")){
			belowDeckTheme.clip=conflictClip;
		}else if(GameMan.main.conditionals.GetValue("BELOWDECK")){// && !GameMan.main.conditionals.GetValue("GRIEVANCE")){
			belowDeckTheme.clip=belowDeckThemeClip;
		}else if (GameMan.main.conditionals.GetValue("UPPERDECK")){
			belowDeckTheme.clip=upperDeckClip;
		}

		if(currentSongState!=belowDeckTheme.clip){
		belowDeckTheme.Play();
		currentSongState=belowDeckTheme.clip;
		}
	}

	void Update(){

		if(RoomMan.main.current.name=="BreadRoom(Clone)" && !GameMan.main.conditionals.GetValue("MAGAZINE")){
			BreadRoomExit.SetActive(true);
		}else if(RoomMan.main.current.name=="BreadRoom(Clone)" && GameMan.main.conditionals.GetValue("MAGAZINE")){
			BreadRoomExitFromMagazine.SetActive(true);
				}else{
				BreadRoomExit.SetActive(false);
			BreadRoomExitFromMagazine.SetActive(false);
				}

		ChangeTheMusic();

		if(GameMan.main.conditionals.GetValue("CUTSCENERUNNING")){
			homeButton.SetActive(false);
			score.SetActive(false);
		}else{
			homeButton.SetActive(true);
			score.SetActive(true);
		}

		if(GameMan.main.conditionals.GetValue("GAMEOVER")){
			startOver.SetActive(true);
			//startOver.GetComponent<CanvasGroupFader>().displaying=true;
		}

		if(GameMan.main.conditionals.GetValue("ENDGAME")){
			endLevelOneCanvas.GetComponent<CanvasGroupFader>().displaying=true;
			endCredits.SetActive(true);
			startButton.SetActive(false);
		}

		if(GameMan.main.conditionals.GetValue("BURNINGSCENE")){
			turnOnBurningScene.SetActive(true);
		}
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
	

	
		//THIS CONTROLS THE THEME MUSIC




		/*if(GameMan.main.conditionals.GetValue("STARTMENU")){
			startMenu.GetComponent<CanvasGroupFader>().displaying=true;
		}*/

		//CONTROLS WHALER SINGING


	
		if(GameMan.main.conditionals.GetValue("OLDMAN")){
			OldManTrans.SetActive(true);
		}

	

	}
}