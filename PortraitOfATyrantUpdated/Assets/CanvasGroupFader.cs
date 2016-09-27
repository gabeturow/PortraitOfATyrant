using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Klak.Math;

public class CanvasGroupFader : MonoBehaviour {

	CanvasGroup cGroup;
	DTween alphaDamper = new DTween(0, 5);
	public bool displaying = false;

	void Start () {
		cGroup = gameObject.ForceGetComponent<CanvasGroup>();
	}

	void Update(){
		alphaDamper.Step(displaying ? 1 : 0);
		cGroup.alpha = alphaDamper;
		cGroup.blocksRaycasts = cGroup.alpha > .01f;
	}

}
