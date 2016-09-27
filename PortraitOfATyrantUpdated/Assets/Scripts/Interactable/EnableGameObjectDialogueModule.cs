using UnityEngine;
using System.Collections;

public class EnableGameObjectDialogueModule : DialogueNode {
	
	public bool valueToSet;
	
	public Transform transform;
	
	public override void Init ()
	{
		
		base.Init ();
		if(transform!=null){
			transform.gameObject.SetActive(valueToSet);
		}
	}
	
	
}
