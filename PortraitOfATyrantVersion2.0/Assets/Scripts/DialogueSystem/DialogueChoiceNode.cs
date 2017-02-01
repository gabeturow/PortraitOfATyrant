﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class DialogueChoice{
	public string label;
	public string responseText;
	public Sprite grievanceImage;
	public DialogueNode nodeChoice;

}


public class DialogueChoiceNode : DialogueNode {
	public static DialogueChoiceNode main;
	public DialogueChoice[] choices;
	public string conditionalState;
	public bool conditionalBool=false;
	public Sprite rightsImage;
	public string rightsLabel;
	public DialogueNode goBackNode;


/*	protected override void DrawConnectors ()
	{
		for(int i = 0; i < choices.Length; i++){
			if (choices[i] != null && choices[i].nodeChoice != null){
				DrawArrow.Connector(transform.position, choices[i].nodeChoice.transform.position - transform.position);
			}
		}
	}
*/

	public override void DoBlock(){

		GameMan.main.conditionals.SetValue(conditionalState, conditionalBool);
		rightsImageMenu=rightsImage;
		rightsLabelMenu=rightsLabel;
		DialogueViewer.main.choicePanel.isActive = true;
		DialogueViewer.main.choicePanel.SetBackFunction(
			()=>{
				GameMan.main.conditionals.SetValue("DIALOGUE", true);
				GameMan.main.conditionals.SetValue("GRIEVANCE", false);
				GameMan.main.conditionals.SetValue("RIGHTS", false);

				this.nextNode = goBackNode;
				this.UnBlock();


			});
		DialogueViewer.main.choicePanel.SetChoices(choices,
			(i)=>{
				this.nextNode = choices[i].nodeChoice;
				DialogueViewer.main.choicePanel.isActive = false;
				this.UnBlock();
			});


	}



}
