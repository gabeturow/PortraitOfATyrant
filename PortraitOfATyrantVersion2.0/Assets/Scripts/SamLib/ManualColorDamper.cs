using UnityEngine;
using System.Collections;

public class ManualColorDamper : MonoBehaviour {

	public Vector4 velocity = Vector4.zero;
	public Vector4 target = Vector4.one;
	public Vector4 current = Vector4.one;
	public float strength = 8.0f;
	public float smoothTime = .2f;

	public void SetAlpha(float alpha){
		var newTarget = target;
		newTarget.w = alpha;
		target = newTarget;
	}

	public void UpdateMe () {
		current = Utilities.Vector4SmoothDamp(current, target, ref velocity, smoothTime);
	}

}

public class ManualColorDamperLite {

	public Vector4 velocity = Vector4.zero;
	public Vector4 target = Vector4.one;
	public Vector4 current = Vector4.one;
	public float strength = 8.0f;
	public float smoothTime = .2f;

	public void SetAlpha(float alpha){
		var newTarget = target;
		newTarget.w = alpha;
		target = newTarget;
	}

	public void UpdateMe () {
		current = Utilities.Vector4SmoothDamp(current, target, ref velocity, smoothTime);
	}
}