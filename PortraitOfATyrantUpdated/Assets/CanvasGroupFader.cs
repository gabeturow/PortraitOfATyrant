using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Klak.Math;

public class CanvasGroupFader : MonoBehaviour {



	private float _fullAlpha = 1;
	public float fullAlpha{ 
		get { return _fullAlpha; } 
		set { _fullAlpha = Mathf.Clamp(value, .1f, 1f); } 
	}

	public bool isVisible{
		get{ return cGroup.alpha > .05f && !displaying; }
	}

	public bool displaying = false;

	CanvasGroup cGroup;
	DTween alphaDamper = new DTween(0, 5);

	public void SetSpeed(float speed){
		alphaDamper.omega = speed;
	}
	void Start () {
		cGroup = gameObject.ForceGetComponent<CanvasGroup>();
	}

	void Update(){
		alphaDamper.Step(displaying ? fullAlpha : 0);
		cGroup.alpha = alphaDamper;
		cGroup.blocksRaycasts = cGroup.alpha > .01f;
	}

}
