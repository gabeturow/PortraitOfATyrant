using UnityEngine;
using System.Collections;

public class PlayDeclarationAnimOnEnter : OnEnterNode{

	public int passageNumber;
	public bool declarationActive=false;

	public override void OnEnter (DialogueNode node)
	{
		UIMan.main.declarationStatus=declarationActive;
	
		UIMan.documentViewer.Show(null, SetFinished);
		UIMan.documentViewer.AnimateReveal(passageNumber);

	}

	void SetFinished(){
		this.finished = true;
	}

}
