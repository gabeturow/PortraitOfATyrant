using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	[SerializeField]
	float floorHeight = 0;
	public float FloorHeight{get{return floorHeight;}}

	public bool staticCamera = false;
	public Vector2 CameraMin = new Vector2(-5f, -5f);
	public Vector2 CameraMax = new Vector2(5f, 5f);


	#if UNITY_EDITOR

	void OnDrawGizmos(){
		var c = Gizmos.color;
		Gizmos.color = new Color(0f, 1f, 1f);

		var start = transform.position;
		start += transform.up * floorHeight;
		var end = start;

		start += transform.right * 10f;
		end += transform.right * -10f;

		Gizmos.DrawLine(start, end);
		Gizmos.color = c;
	}

	#endif


}
