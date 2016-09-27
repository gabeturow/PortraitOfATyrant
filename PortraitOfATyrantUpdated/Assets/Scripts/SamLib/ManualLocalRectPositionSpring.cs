using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManualLocalRectPositionSpring : MonoBehaviour {

	RectTransform rect;

	public Vector2 velocity = Vector3.zero;
	public Vector2 target = Vector3.zero;
	public Vector2 current{get{return rect.anchoredPosition;}set{rect.anchoredPosition= value;}}
	public float strength = 8.0f;
	public float dampingRatio = .8f;

	public bool randomizer;

	void Awake(){
		this.rect = gameObject.ForceGetComponent<RectTransform>();
	}


	public void UpdateMe () {
		if (randomizer){
			velocity += Random.insideUnitCircle * 10f;
			randomizer = false;
		}
		current = Utilities.SimpleHarmonicMotion(current, target, ref velocity, strength, dampingRatio);
	}

}
