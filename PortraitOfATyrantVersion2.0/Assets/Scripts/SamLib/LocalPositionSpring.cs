using UnityEngine;
using System.Collections;

public class LocalPositionSpring : MonoBehaviour {

	public Vector3 velocity = Vector3.zero;
	public Vector3 target = Vector3.one;
	public Vector3 current{get{return transform.localPosition;}set{transform.localPosition= value;}}
	public float strength = 8.0f;
	public float dampingRatio = .8f;

	public bool randomizer;

	// Update is called once per frame
	void LateUpdate () {
		if (randomizer){
			velocity += Random.insideUnitSphere * 10f;
			randomizer = false;
		}
		current = Utilities.SimpleHarmonicMotion(current, target, ref velocity, strength, dampingRatio);
	}

}
