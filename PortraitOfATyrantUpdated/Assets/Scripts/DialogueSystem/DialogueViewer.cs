using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueViewer : MonoBehaviour {
	public static DialogueViewer main;
	public GameObject dialogueGameObject;
	public ChoicePanel choicePanel{get; private set;}
	public GameObject choicePrompt;

	public DialogueTree currentDialogue{get; private set;}
	public Text characterName;
	public Text dialogueText;
	public Image characterPortrait;
	public AudioSource audioSource;
	ManualLocalScaleSpring scaleSpring;
	ManualFloatDamper alphaDamper;
	CanvasGroup canvasGroup;

	const float advancerBuffer = .5f;
	float advancerBufferTimer = 0f;
	float autoAdvancer = 0f;
	const float autoAdvancerDelay = .05f;
	const float playDelay = .01f;

	void Awake(){
		main = this;
		canvasGroup = GetComponent<CanvasGroup>();
		scaleSpring = GetComponent<ManualLocalScaleSpring>();
		alphaDamper = new ManualFloatDamper(0f,.10f);
		choicePanel = GetComponentInChildren<ChoicePanel>();
	}


	public void PlayDialogue(DialogueTree dialogue){
		var d = ((GameObject)Instantiate(dialogue.gameObject)).GetComponent<DialogueTree>();
		d.Init();
		this.currentDialogue = d;
		TryAdvance();
		this.scaleSpring.velocity += new Vector3(2f, 2f, 2f);
		this.alphaDamper.Target = 1f;
	}


	void Update(){

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

		scaleSpring.UpdateMe();
		alphaDamper.Update();
		canvasGroup.alpha = alphaDamper.Value;

		if(choicePanel.isActive){
			
			if(choicePanel.riddleActive){
				choicePanel.riddleText.text=dialogueText.text;
			}else{
				choicePrompt.GetComponent<Image>().enabled=false;
			}
			dialogueGameObject.SetActive(false);
		}else{
			dialogueGameObject.SetActive(true);
			choicePrompt.GetComponent<Image>().enabled=true;
		}
	}

	void TryAdvance(){
		choicePanel.isActive = false;
		if (currentDialogue.IsComplete){
			alphaDamper.Target = 0f;
			alphaDamper.Speed = .1f;
			scaleSpring.velocity += Vector3.one * -1f;
			audioSource.Stop();
			Destroy(currentDialogue.gameObject);
			currentDialogue = null;
		}
		else{
			var currentChar = CurrentCharacter;
			currentDialogue.AdvanceDialogue();
			if (currentChar != CurrentCharacter){
				scaleSpring.velocity += new Vector3(0f, -18f, 0f);
			}
			audioSource.Stop();
			DrawCurrentMessage();
			advancerBufferTimer = advancerBuffer;
		}
	}

	void DrawCurrentMessage(){
		if (currentDialogue.IsBlocked){
			return;
		}
		else{
			choicePanel.isActive = false;
			audioSource.clip = currentDialogue.CurrentMessage.audioClip;
			if (CurrentCharacter == null){
				Debug.LogError("No character assigned for node " + currentDialogue.current.gameObject.name);
			}
			this.characterPortrait.sprite = CurrentCharacter.portrait;
			this.characterName.text = CurrentCharacter.name;
			dialogueText.text = currentDialogue.CurrentMessage.text;
			audioSource.PlayDelayed(playDelay);
			autoAdvancer = (audioSource.clip == null ? 10f : audioSource.clip.length-1) + autoAdvancerDelay;
		}
	}

	public DialogueCharacter CurrentCharacter{
		get{return currentDialogue.current.character ?? currentDialogue.defaultCharacter;}
	}

}
