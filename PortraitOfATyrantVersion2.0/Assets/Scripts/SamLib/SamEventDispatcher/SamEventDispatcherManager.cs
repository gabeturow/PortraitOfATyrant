using UnityEngine;
using System.Collections;

public class SamEventDispatcherManager : MonoBehaviour {

	public bool slowUpdate = true;

	void Update () {
		if (slowUpdate){
			SamEventDispatcher.SlowUpdate();
		}
		else{
			SamEventDispatcher.FastUpdate();
		}
	}
}
