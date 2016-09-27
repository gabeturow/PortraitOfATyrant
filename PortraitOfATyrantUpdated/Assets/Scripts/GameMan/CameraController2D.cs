using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Klak.Math;

public class CameraController2D : MonoBehaviour {
	public static CameraController2D main;

	const float TRACKING_STRENGTH = 6;

	public bool isActive = false;
	public Vector2 disabledPosition = Vector2.zero;

	public bool useMinMax;
	public Vector2 min, max;

	private List<TrackedObj> tracking = new List<TrackedObj>();
	private List<TrackedObj> temporaryTracking = new List<TrackedObj>();

	private DTweenVector2 position = new DTweenVector2(Vector2.zero, TRACKING_STRENGTH);
	public Vector2 target;

	void Awake(){
		main = this;
	}


	public void Track(GameObject g){
		if (!IsTrackingInternal(g, tracking)){
			TrackInternal(g, tracking);
		}
	}

	public void TempTrack(GameObject g){
		if (!IsTrackingInternal(g, tracking)){
			TrackInternal(g, temporaryTracking);
		}
	}

	public bool IsTracking(GameObject g){
		return IsTrackingInternal(g, tracking);
	}

	public bool IsTempTracking(GameObject g){
		return IsTrackingInternal(g, temporaryTracking);
	}

	public bool RemoveTracking(GameObject g){
		return RemoveTrackingInternal(g, tracking);
	}

	public void RemoveTempTracking(){
		temporaryTracking.Clear();
	}

	internal bool RemoveTrackingInternal(GameObject g, List<TrackedObj> list){
		for(int i = 0; i < tracking.Count; i++){
			if (tracking[i].gameObject != null && tracking[i].gameObject == g){
				tracking.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	internal void TrackInternal(GameObject g, List<TrackedObj> list){
		list.Add(g);
	}

	internal bool IsTrackingInternal(GameObject g, List<TrackedObj> list){
		for(int i = 0; i < list.Count; i++){
			if (list[i].gameObject != null && list[i].gameObject == g){
				return true;
			}
		}
		return false;
	}


	// Update is called once per frame
	void LateUpdate () {
		if (!isActive){ return; }
		if (tracking.Count + temporaryTracking.Count > 0){
			target = GetAverageTracking();
			if (useMinMax){
				target.x = Mathf.Clamp(target.x, min.x, max.x);
				target.y = Mathf.Clamp(target.y, min.y, max.y);
			}
			position.Step(target);
			Vector2 newPos = position;
			transform.position = newPos;
		}
	}

	Vector2 GetAverageTracking(){
		Vector2 ans = Vector2.zero;
		for(int i = 0; i < tracking.Count; i++){
			ans += tracking[i].GetPosition();
		}
		for(int i = 0; i < temporaryTracking.Count; i++){
			ans += temporaryTracking[i].GetPosition();
		}
		return ans / (tracking.Count + temporaryTracking.Count);
	}



	public class TrackedObj{
		public GameObject gameObject;
		public Vector2 pos;
		public TrackedObj(GameObject g){
			this.gameObject = g;
		}
		public TrackedObj(Vector2 pos){
			this.pos = pos;
		}
		public Vector2 GetPosition(){
			if (gameObject != null) return gameObject.transform.position;
			return pos;
		}
		public static implicit operator Vector2(TrackedObj o){
			return o.GetPosition();
		}
		public static implicit operator TrackedObj(Vector2 pos){
			return new TrackedObj(pos);
		}
		public static implicit operator TrackedObj(GameObject g){
			return new TrackedObj(g);
		}

	}

}
