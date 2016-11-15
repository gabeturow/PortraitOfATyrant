using UnityEngine;
using System.Collections;

public class PlayDeclarationAnimOnEnter : OnEnterNode{

	public int passageNumber;
	public bool declarationActive=true;

	public override void OnEnter (DialogueNode node)
	{
		UIMan.declarationViewer.Show(null, SetFinished);
		UIMan.declarationViewer.AnimateReveal(passageNumber);
	}

	void SetFinished(){
		this.finished = true;
	}

}
