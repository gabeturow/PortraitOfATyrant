using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueViewer : MonoBehaviour {
	public static DialogueViewer main;
	public GameObject dialogueGameObject;
	public ChoicePanel choicePanel{get; private set;}
	public GameObject choicePromptDialogue;
	public GameObject choicePromptGrievance;
	public GameObject characterOne;


	public DialogueTree currentDialogue{get; private set;}
	public Text characterName;
	public Text dialogueText;
	public Text riddlePromptNew;
	public Image characterPortrait;
	public AudioSource audioSource;
	//ManualLocalScaleSpring scaleSpring;
	//ManualFloatDamper alphaDamper;
	public CanvasGroupFader fader;
	public CanvasGroupFader faderNew;
	public GameObject cameraObj;


	const float advancerBuffer = .5f;
	float advancerBufferTimer = 0f;
	float autoAdvancer = 0f;
	const float autoAdvancerDelay = .05f;
	const float playDelay = .01f;

	void Awake(){
		main = this;
		//canvasGroup = GetComponent<CanvasGroup>();
	//	scaleSpring = GetComponent<ManualLocalScaleSpring>();
	//	alphaDamper = new ManualFloatDamper(0f,.10f);
		choicePanel = GetComponentInChildren<ChoicePanel>();
		fader = gameObject.GetComponentInChildren<CanvasGroupFader>();
		faderNew=dialogueGameObject.ForceGetComponent<CanvasGroupFader>();

	}

	public void PlayDialogue(DialogueTree dialogue){
		//FADE ANY OPEN TOAST MESSAGES////
		Toaster.main.Hide();
		////////////////////////////////

		var d = ((GameObject)Instantiate(dialogue.gameObject)).GetComponent<DialogueTree>();
		d.Init();
		this.currentDialogue = d;

		TryAdvance();
		faderNew.displaying = true;

		//Move the camera around when you click to start a dialogue
		cameraObj.GetComponent<CameraMoves>().cameraGo=true;
	}


	void Update(){

		//Move the camera around when you click to start a dialogue
	

		if (autoAdvancer > 0) autoAdvancer -= Time.deltaTime;
		if (advancerBufferTimer > 0) advancerBufferTimer -= Time.deltaTime;

		if (currentDialogue != null && currentDialogue.IsBlocked != true){
			if (audioSource.clip != null && !audioSource.isPlaying && autoAdvancer <= 0f){
				TryAdvance();
			}
			if (Input.GetMouseButtonDown(0) && advancerBufferTimer <= 0f){
				TryAdvance();
			}
		}

		//scaleSpring.UpdateMe();
		//alphaDamper.Update();

		//canvasGroup.alpha = alphaDamper.Value;
				
		if(choicePanel.isActive){
			if(GameMan.main.conditionals.GetValue("RIDDLE")){
				riddlePromptNew.text=dialogueText.text;
			}else{
				riddlePromptNew.text="";
				choicePromptDialogue.GetComponent<Image>().enabled=false;
			}
			faderNew.displaying =false;
			//dialogueGameObject.SetActive(false);
		
		}else{
			//faderNew.displaying =true;
			choicePromptDialogue.GetComponent<Image>().enabled=true;
		}
	}

	void TryAdvance(){
		fader.displaying =false;
		faderNew.displaying =false;
		choicePanel.isActive = false;
		if (currentDialogue.IsComplete){
			//alphaDamper.Target = 0f;
			//alphaDamper.Speed = .1f;
			//scaleSpring.velocity += Vector3.one * -2f;
			audioSource.Stop();
			Destroy(currentDialogue.gameObject);
			currentDialogue = null;
			//Move the camera back to normal position when you end a dialogue
			cameraObj.GetComponent<CameraMoves>().cameraGo=false;
		}
		else{
			faderNew.displaying =true;
			var currentChar = CurrentCharacter;
			currentDialogue.AdvanceDialogue();
			if (currentChar != CurrentCharacter){
				//scaleSpring.velocity += new Vector3(0f, -5f, 0f);
			}
			audioSource.Stop();
			DrawCurrentMessage();
			advancerBufferTimer = advancerBuffer;
		}
	}

	void DrawCurrentMessage(){
		
		if (currentDialogue.IsBlocked){
			faderNew.displaying =true;
			return;

		}
		else{
			//faderNew.displaying =false;
			//choicePanel.isActive = false;
			choicePanel.isActive = false;
			audioSource.clip = currentDialogue.CurrentMessage.audioClip;
			if (CurrentCharacter == null){
				Debug.LogError("No character assigned for node " + currentDialogue.current.gameObject.name);
			}
			this.characterPortrait.sprite = CurrentCharacter.portrait;
			this.characterName.text = CurrentCharacter.name;
			dialogueText.text = currentDialogue.CurrentMessage.text;
			audioSource.PlayDelayed(playDelay);
			autoAdvancer = (audioSource.clip == null ? 10f : audioSource.clip.length-.2f) + autoAdvancerDelay;
		}
	}

	public DialogueCharacter CurrentCharacter{
		get{return currentDialogue.current.character ?? currentDialogue.defaultCharacter;}
	}

}
