using UnityEngine;
using System.Collections;

public abstract class InteractionModule : MonoBehaviour{

	public virtual bool CheckInteract(){ 
		return true; 
	}

	public virtual void OnInteract(){

	}

}
