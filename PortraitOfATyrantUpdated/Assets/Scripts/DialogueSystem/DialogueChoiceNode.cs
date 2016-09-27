using UnityEngine;
using System.Collections;

[System.Serializable]
public class DialogueChoice{
	public string responseText;
	public DialogueNode nodeChoice;

}

public class DialogueChoiceNode : DialogueNode {

	public DialogueChoice[] choices;
	public bool grievanceDialogue;
	public bool rightsDialogue;
	public bool riddleDialogue;


	protected override void DrawConnectors ()
	{
		for(int i = 0; i < choices.Length; i++){
			if (choices[i] != null && choices[i].nodeChoice != null){
				DrawArrow.Connector(transform.position, choices[i].nodeChoice.transform.position - transform.position);
			}
		}
	}


	public override void DoBlock(){
		DialogueViewer.main.choicePanel.isActive = true;

		if(grievanceDialogue){
			DialogueViewer.main.choicePanel.grievanceActive=true;
			DialogueViewer.main.choicePanel.rightsActive=false;
			DialogueViewer.main.choicePanel.riddleActive=false;
		}
		if(rightsDialogue){
			DialogueViewer.main.choicePanel.grievanceActive=false;
			DialogueViewer.main.choicePanel.rightsActive=true;
			DialogueViewer.main.choicePanel.riddleActive=false;
		}
		if(riddleDialogue){
			DialogueViewer.main.choicePanel.grievanceActive=false;
			DialogueViewer.main.choicePanel.rightsActive=false;
			DialogueViewer.main.choicePanel.riddleActive=true;
		}

		DialogueViewer.main.choicePanel.SetChoices(choices,
			(i)=>{
				this.nextNode = choices[i].nodeChoice;
				DialogueViewer.main.choicePanel.isActive = false;
				this.UnBlock();
			});
	}


}
