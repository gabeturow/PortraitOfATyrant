using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedDestroyer : MonoBehaviour {

	private static Queue<Object> q = new Queue<Object>();

	void LateUpdate(){
		Destroy(q.Dequeue());
	}


}
