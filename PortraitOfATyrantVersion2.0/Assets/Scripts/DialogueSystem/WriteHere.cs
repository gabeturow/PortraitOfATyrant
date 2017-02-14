using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WriteHere : MonoBehaviour {

	public string writeThisText;
	public int numberofLines;
	public DialogueNode currentNode;
	public string currentCharacter;
	string SoundEffect="Sound Effect";
	string currentCharacterName;
	public bool isARiddle=false;
	public DialogueNode choiceNodeTransNew;
	public DialogueNode currentNodeNew;

	void Start(){
		Writing();
	}
		

	void RunThroughNodeListUntilHitBlankNode(DialogueNode ThisNodeLine){
		numberofLines=ThisNodeLine.lines.Length;
		//Look through next node
		for(int x=0; x<numberofLines; x++){

			if(ThisNodeLine.character!=null){
				string characterName="<br><br>    <b></ul>"+ThisNodeLine.character.name.ToString().ToUpper()+"</b></ul><br>";
				if(x!=0){characterName="<br>";}
				writeThisText +=""+characterName+"   "+ThisNodeLine.lines[x].text;
				currentCharacterName=characterName;
			} else{
				writeThisText +="<br><br>   <b></ul>"+SoundEffect.ToString().ToUpper()+"</b></ul><br>   "+ThisNodeLine.lines[x].text;
			}
		}
		return;
	}
	int whichChoice=0;
	int whichChoiceNew=0;
	public void Writing(){
		

		//Find the root node
		numberofLines=GetComponent<DialogueTree>().root.lines.Length;
		//Look through the root node
		for(int x=0; x<numberofLines; x++){
			writeThisText +=""+GetComponent<DialogueTree>().root.lines[x].text;
		}
		//Look for the next node. If there isn't one, Break.
		currentNode=GetComponent<DialogueTree>().root.nextNode;

		while(currentNode!=null){
			
			RunThroughNodeListUntilHitBlankNode(currentNode);

			//Find next node
			currentNode=currentNode.nextNode;

		
				if(currentNode.GetComponent<DialogueChoiceNode>()!=null){
					DialogueNode choiceNodeTrans=currentNode;
				 
					while(whichChoice<4){
					writeThisText +="<b><br><br><u>Grievance #</b> "+(whichChoice+1f)+"</u>/";
						currentNode=choiceNodeTrans.GetComponent<DialogueChoiceNode>().choices[whichChoice].nodeChoice;
					writeThisText +="<i>"+choiceNodeTrans.GetComponent<DialogueChoiceNode>().choices[whichChoice].responseText+"</i>/";
					writeThisText +=""+choiceNodeTrans.GetComponent<DialogueChoiceNode>().choices[whichChoice].grievanceImage.name+"/";
					writeThisText +=""+choiceNodeTrans.GetComponent<DialogueChoiceNode>().choices[whichChoice].label+"/";


						while(currentNode!=null){
							RunThroughNodeListUntilHitBlankNode(currentNode);
						currentNode=currentNode.nextNode;
						if(currentNode!=null && currentNode.character!=null){
							if(currentNode.character.name=="Rights"){
								choiceNodeTransNew=currentNode;
								while(whichChoiceNew<4){
									writeThisText +="<b><br><br><u>Right #</b> "+(whichChoiceNew+1f)+"</u>/";
									currentNodeNew=choiceNodeTransNew.GetComponent<DialogueChoiceNode>().choices[whichChoiceNew].nodeChoice;
									writeThisText +="<i>"+choiceNodeTransNew.GetComponent<DialogueChoiceNode>().choices[whichChoiceNew].responseText+"</i>/";
									writeThisText +=""+choiceNodeTransNew.GetComponent<DialogueChoiceNode>().rightsImage.name+"/";
									writeThisText +=""+choiceNodeTransNew.GetComponent<DialogueChoiceNode>().rightsLabel+"/";

									while(currentNodeNew!=null){
										RunThroughNodeListUntilHitBlankNode(currentNodeNew);
										currentNodeNew=currentNodeNew.nextNode;
									}
									whichChoiceNew++;
								}
							}
						}
					}	
					whichChoice++;
					}	
			}
		}
	}
}
	

