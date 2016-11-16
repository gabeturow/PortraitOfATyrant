using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Klak.Math;

public class AnimatedPassage : MonoBehaviour {

	#region vars

	Image image;

	Image imageEnd;

	const float ANIM_SPEED = 10;

	DTweenVector2 scaleDamper;
	bool isAnimating;

	#endregion

	void Awake(){
		image = transform.FindChild("PassageBegin").GetComponent<Image>();
		imageEnd = transform.FindChild("PassageEnd").GetComponent<Image>();

		imageEnd.gameObject.SetActive(false);
		scaleDamper = new DTweenVector2(image.rectTransform.sizeDelta, ANIM_SPEED);
		isAnimating = false;
	}


	public void Hide(){
		image.CrossFadeAlpha(0f, 0f, true);
	}

	public void AnimateIn(float speed = ANIM_SPEED){
		//fade image in
		image.CrossFadeAlpha(1f, .5f, true);

		//fade image out after seconds
		Invoke("StartAnimation", 1f);
	}

	void StartAnimation(){
		//UIMan.main.TurnOnDeclaration();
		isAnimating = true;
	}


	void Update(){
		if (isAnimating) {
			FadeInUpdate();
		}
	}

	void FadeInUpdate(){
		scaleDamper.Step(imageEnd.rectTransform.sizeDelta);
		image.rectTransform.sizeDelta = scaleDamper;
	}




}
