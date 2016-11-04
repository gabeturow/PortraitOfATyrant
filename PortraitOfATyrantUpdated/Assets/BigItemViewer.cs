﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Shows a big sprite with a close button. Optional callback for when close button is pressed.
/// </summary>
public class BigItemViewer : MonoBehaviour {

	#region properties

	public static BigItemViewer main { get; private set; }
	public bool active { get { return fader.displaying; } }

	#endregion



	#region vars 

	float imageScaleSpeed = 5f;

	CanvasGroupFader fader;
	Button backButton;
	Image itemImage;

	System.Action onFinishInspect;

	#endregion



	#region publics

	public void Show(Sprite itemSprite, System.Action onFinishInspect = null){
		this.itemImage.sprite = itemSprite;
		fader.displaying = true;
		SetOnFinishCallback(onFinishInspect);
	}

	public void Hide(){
		fader.displaying = false;
		FireOnFinishInspectCallback();
	}

	#endregion


	#region monobehaviour


	void Awake(){
		EnsureSingleton();
		fader = GetComponent<CanvasGroupFader>();
		fader.SetSpeed(40f);
		itemImage = GetComponentInChildren<Image>();
		backButton = GetComponentInChildren<Button>();
		backButton.onClick.AddListener(Hide);
	}

	void Update(){
		UpdateImageScale();
	}

	#endregion


	void UpdateImageScale(){
		var targetScale = this.active ? Vector3.one : Vector3.zero;
		var currentScale = this.itemImage.transform.localScale;
		var newScale = Vector3.Lerp(currentScale, targetScale, Time.deltaTime * imageScaleSpeed);
		this.itemImage.transform.localScale = newScale;
	}

	void SetOnFinishCallback(System.Action onFinishInspect){
		this.onFinishInspect = onFinishInspect;
	}

	void FireOnFinishInspectCallback(){
		if (onFinishInspect != null) {
			onFinishInspect();
			onFinishInspect = null;
		}
	}

	void EnsureSingleton(){
		if (main != null) {
			Debug.LogError("BigItemViewer already exists!");
			Destroy(this.gameObject);
			return;
		}
		main = this;
	}

}
