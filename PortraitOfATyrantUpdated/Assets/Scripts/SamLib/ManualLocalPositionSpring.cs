using UnityEngine;
using System.Collections;

public class ManualLocalPositionSpring : MonoBehaviour {

	public Vector3 velocity = Vector3.zero;
	public Vector3 target = Vector3.zero;
	public Vector3 current{get{return transform.localPosition;}set{transform.localPosition= value;}}
	public float strength = 8.0f;
	public float dampingRatio = .8f;

	public bool randomizer;


	public void UpdateMe () {
		if (randomizer){
			velocity += Random.insideUnitSphere * 10f;
			randomizer = false;
		}
		current = Utilities.SimpleHarmonicMotion(current, target, ref velocity, strength, dampingRatio);
	}

}
