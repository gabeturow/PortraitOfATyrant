using UnityEngine;
using System.Collections;

public abstract class OnEnterNode : MonoBehaviour {

	protected bool finished;
	public abstract void OnEnter(DialogueNode node);
	public virtual bool IsFinished(){
		return finished;
	}

}
