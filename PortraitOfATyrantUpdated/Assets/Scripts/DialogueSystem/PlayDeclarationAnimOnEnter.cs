using UnityEngine;
using System.Collections;

public class PlayDeclarationAnimOnEnter : OnEnterNode{

	enum Document{Declaration, BillOfRights}

	[SerializeField]
	Document document;

	public int passageNumber;


	public override void OnEnter (DialogueNode node)
	{
		DocumentViewer documentViewer = GetDocumentViewer();

		documentViewer.Show(null, SetFinished);
		documentViewer.AnimateReveal(passageNumber);
	}


	DocumentViewer GetDocumentViewer(){

		DocumentViewer answer = null;

		switch (this.document) {

			case Document.Declaration:
				answer = UIMan.declaration;
				break;
			case Document.BillOfRights:
				answer = UIMan.billOfRights;
				break;
		}

		return answer;
	}

	void SetFinished(){
		this.finished = true;
	}

}
