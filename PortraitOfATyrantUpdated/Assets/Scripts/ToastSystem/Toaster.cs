using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Toaster : MonoBehaviour {

	public static Toaster main;

	const float SLIDE_DELTA = 210f;
	const float SHOW_DURATION = 20f;

	Image img;
	Text text;
	CanvasGroup cGroup;

	ManualColorDamperLite colorDamper;
	ManualLocalPositionSpring posDamper;
	ManualFloatDamper alphaDamper;

	Vector3 showPos;
	Vector3 hidePos;

	float showTimer;


	void Awake(){
		main = this;
		img = GetComponentInChildren<Image>();
		text = GetComponentInChildren<Text>();
		cGroup = GetComponentInChildren<CanvasGroup>();

		colorDamper = new ManualColorDamperLite();
		posDamper = img.GetComponent<ManualLocalPositionSpring>();
		colorDamper.target = Color.black;
		colorDamper.current = Color.black;
		alphaDamper = new ManualFloatDamper(0f, .1f);

		showPos = img.transform.localPosition;
		hidePos = showPos + new Vector3(0f, SLIDE_DELTA, 0f);
		posDamper.target = hidePos;
	}

	public void Show(string message){
		alphaDamper.Target = 1f;
		text.text = message;
		posDamper.target = showPos;
		showTimer = SHOW_DURATION;
	}

	public void Hide(){
		alphaDamper.Target = 0f;
		posDamper.target = hidePos;
	}


	// Update is called once per frame
	void Update () {
		posDamper.UpdateMe();

		colorDamper.UpdateMe();
		img.color = colorDamper.current;

		alphaDamper.Update();
		cGroup.alpha = alphaDamper.Value;

		if (showTimer > 0){
			showTimer -= Time.deltaTime;
			if (showTimer <= 0){
				Hide();
			}
		}
	}
}
