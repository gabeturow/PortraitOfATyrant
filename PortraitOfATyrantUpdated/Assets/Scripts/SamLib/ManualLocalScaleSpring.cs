using UnityEngine;
using System.Collections;

public class ManualLocalScaleSpring : MonoBehaviour {

	public Vector3 velocity = Vector3.zero;
	public Vector3 target = Vector3.one;
	public Vector3 current{get{return transform.localScale;}set{transform.localScale= value;}}
	public float strength = 8.0f;
	public float dampingRatio = .8f;


	public void UpdateMe () {
		current = Utilities.SimpleHarmonicMotion(current, target, ref velocity, strength, dampingRatio);
	}

}
