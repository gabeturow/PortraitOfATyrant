using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class RunSlideshow : MonoBehaviour {
	public bool endingTrue;
	public float speed = 1f;
	public Sprite[] slides;
	public Sprite currentSlide;
	public int x=0;
	bool dummy=false;
	CanvasGroupFader fader;
	CanvasGroupFader fadeIt;
	public bool fadeOut2=true;
	public GameObject backdrop;
	public GameObject button;
	public GameObject slideshowFaderObject;
	ConditionalController newCondition;
	public bool fadeOut=true;
	public bool music=true;
	public GameObject levelContent;

	//public GameObject condition;

	// Use this for initialization

	void Awake(){
		this.GetComponent<Image>().sprite=currentSlide;
		fader=gameObject.ForceGetComponent<CanvasGroupFader>();
		fadeIt=backdrop.GetComponent<CanvasGroupFader>();
		//newCondition=condition.GetComponent<ConditionalController>();
		fader.displaying=true;

	

	}
IEnumerator Start() {
		yield return StartCoroutine(GoSlides(speed));
	
	//	GameMan.main.conditionals.SetValue("GRIEVANCE", true);
	//	GameMan.main.conditionals.SetValue("BELOWDECK", false);

	}
	 
	void Update(){
		this.GetComponent<Image>().sprite=currentSlide;
		fader.displaying=fadeOut;
		fadeIt.displaying=fadeOut2;


	}

	public IEnumerator GoSlides(float waitTime){

		GameMan.main.conditionals.SetValue("CONFLICT", true);
		//if(!endingTrue){
		fadeOut=true;
		yield return new WaitForSeconds(waitTime*1.5f);
		fadeOut=false;
		yield return new WaitForSeconds(waitTime/2f);
		//}
		for(x=0;x<slides.Length-2; x++){
			if(slides[x]!=null){
				fadeOut=true;
				currentSlide=slides[x];
				yield return new WaitForSeconds(waitTime);
				fadeOut=false;
				yield return new WaitForSeconds(.8f);
			}

		}
		
		if(!endingTrue){
		GameMan.main.conditionals.SetValue("CONFLICT", false);
		GameMan.main.conditionals.SetValue("BELOWDECK", true);
		}
		//cmusic=false;
		fadeOut=true;
		currentSlide=slides[slides.Length-2];
		yield return new WaitForSeconds(waitTime);
		fadeOut=false;
		yield return new WaitForSeconds(.8f);

		fadeOut=true;
		currentSlide=slides[slides.Length-1];
		yield return new WaitForSeconds(waitTime);
		fadeOut=false;
	
		yield return new WaitForSeconds(.8f);

		fadeOut=true;
		fadeOut2=true;

			fadeOut=false;
		button.SetActive(false);
		yield return new WaitForSeconds(1);
		slideshowFaderObject.GetComponent<CanvasGroupFader>().displaying=false;
		if(levelContent!=null){
		levelContent.SetActive(true);
		}
		//yield return StartCoroutine(ReturnGrievanceVolume());
//		ConditionalController.main.belowDeckTheme.enabled=true;
//		ConditionalController.main.conflictTheme.enabled=false;


}
	IEnumerator ReturnGrievanceVolume(){
		yield return new WaitForSeconds(2);
	//	newCondition.conflictTheme.volume=.02f;

	}


}