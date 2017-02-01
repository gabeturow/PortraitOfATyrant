using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour {

	public float maxSpeed = 4f;
	public float acceleration = 7f;
	public float deceleration_threshold = .5f;
	public float stop_threshold = .1f;

	[SerializeField]
//	float heightOffset = .4f;

	public float currentHeight{get; set;}

	public Vector2 currentVelocity{get; private set;}

	public Vector2 target;
	public Rigidbody2D rbody{get; private set;}

	void Awake(){
		rbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		MovementUpdate();
	}

	void MovementUpdate(){
//		currentHeight=Side.localPosition.y;
		Vector2 delta = target - rbody.position;
		delta.y = 0;

		Vector2 deltaNorm = delta.normalized;
		float dist = Mathf.Abs(target.x - rbody.position.x);

		currentVelocity += deltaNorm * acceleration * Time.deltaTime;
		float currentSpeed = currentVelocity.magnitude;

		if (dist < deceleration_threshold){
			float dist_T = Mathf.Clamp((dist), 0, deceleration_threshold) /deceleration_threshold;
			float maxVelocity = Mathf.Lerp(0, maxSpeed, dist_T);
			maxVelocity = Mathf.Min(maxVelocity, currentSpeed);
			currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxVelocity);
		}
		if (dist < stop_threshold){
			currentVelocity = Vector2.zero;
		}

		currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
		Vector3 movement = currentVelocity * Time.deltaTime;

		Vector3 finalPos = transform.position + movement;
	//	finalPos.y = currentHeight;// + heightOffset;
	//	currentHeight-=heightOffset;
		transform.position = finalPos;
	}


	#if UNITY_EDITOR

	void OnDrawGizmos(){
		var c = Gizmos.color;
		Gizmos.color = new Color(0f, 1f, 1f);

		var start = transform.position;
		start -= transform.up;// * heightOffset;
		var end = start;

		start += transform.right * 2f;
		end += transform.right * -2f;

		Gizmos.DrawLine(start, end);
		Gizmos.color = c;
	}

	#endif
}
