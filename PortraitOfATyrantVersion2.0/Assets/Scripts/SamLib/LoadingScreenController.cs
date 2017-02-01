using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour {

	public static LoadingScreenController main;
	float fadeAngularFreq = 15f;
	float fadeDamping = 1f;

	public Image fadeImage{get; private set;}
	Action onLoadAction = null;
	Action onFinishAction = null;

	private CanvasColorSpring colorSpring;
	float wait = 0f;
	private bool fading = false;
	private bool hasLoaded = false;

	void Awake () {
		if (main != null){
			Destroy(main.gameObject);
		}
		main = this;
		fadeImage = gameObject.ForceGetComponent<Image>();
		fadeImage.rectTransform.anchorMin = new Vector2(0f,0f);
		fadeImage.rectTransform.anchorMax = new Vector2(1f,1f);
		fadeImage.color = new Color(0,0,0,1);

		colorSpring = fadeImage.gameObject.AddComponent<CanvasColorSpring> ();
		colorSpring.strength = fadeAngularFreq;
		colorSpring.dampingRatio = fadeDamping;
	}

	public void FadeToBlack(Action onLoad, Action onFinish = null){
		onLoadAction = onLoad;
		onFinishAction = onFinish;
		fading = true;
		hasLoaded = false;

		transform.SetAsLastSibling ();
	}

	void Update () {

		if (wait > 0f) {
			wait -= Time.deltaTime;
			return;
		}
		if (!fading) 
			return;

		Color targetColor = hasLoaded ? Color.clear : Color.black;
		colorSpring.target = targetColor;

		if (Mathf.Abs(colorSpring.GetCurrentRenderColor ().a - targetColor.a) < .005f) {
			colorSpring.SetCurrentRenderColor (targetColor);
			if (!hasLoaded) {
				hasLoaded = true;
				if (onLoadAction != null)
					onLoadAction ();
				wait = 1f;

			} 
			else {
				fading = false;
				if (onFinishAction != null)
					onFinishAction ();
			}
		}

		fadeImage.enabled = colorSpring.GetCurrentRenderColor().a > .001f;


	}
}
