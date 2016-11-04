using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BigItemViewer : MonoBehaviour {
	public static BigItemViewer main { get; private set; }
	public bool active { get { return fader.displaying; } }

	const float SCALESPEED = 5f;

	CanvasGroupFader fader;

	Button backButton;
	Image itemImage;

	System.Action onFinishInspect;



	public void Show(Sprite itemSprite, System.Action onFinishInspect = null){
		this.itemImage.sprite = itemSprite;
		fader.displaying = true;
		SetOnFinishCallback(onFinishInspect);
	}

	public void Hide(){
		fader.displaying = false;
		FireOnFinishInspectCallback();
	}


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

	void UpdateImageScale(){
		var targetScale = this.active ? Vector3.one : Vector3.zero;
		var dist = Vector3.Distance(this.itemImage.transform.localScale, targetScale);
		this.itemImage.transform.localScale = Vector3.Lerp(this.itemImage.transform.localScale, targetScale, Time.deltaTime * SCALESPEED);
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
