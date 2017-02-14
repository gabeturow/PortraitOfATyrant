//using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WriteBack : MonoBehaviour {
	
	public TextAsset dictionaryTextFile;
	public string theWholeFileAsOneLongString;
	new List<string> eachLine = new List<string>();
	public GameObject prefab;
	public GameObject choiceNodePrefab;
	public GameObject parentPrefab;
	public GameObject childPrefab;
	public GameObject newChoiceInst;
	public GameObject newChoiceRightInst;
	public GameObject testThingToEnable;

	public DialogueCharacter BrianaPrefab;
	public DialogueCharacter DudingstonPrefab;
	public DialogueCharacter ActionPrefab;
	public DialogueCharacter AmericanPrefab;
	public DialogueCharacter ChoiceCharacterPrefab;
	public DialogueCharacter SmugglerPrefab;
	public DialogueCharacter CookPrefab;

	private int lineCounter=0;
	private DialogueNode currentNode;
	public DialogueNode startNode;
	public bool grievanceOn=false;
	public bool rightOn=false;
	public bool grievanceTwoThroughFourOn=false;
	public bool rightTwoThroughFourOn=false;
	public bool grievanceJustOn=false;
	public int choiceNumberGrievance=0;
	public int choiceNumberRight=0;
	int whichChoiceCounter=0;
	Char delimiter = '/';
	String[] substrings;
	string substring;


	public Animator upperDeckAnimator;


	void Start()
	{
		theWholeFileAsOneLongString = dictionaryTextFile.text;

		//Split it into Lines
		eachLine.AddRange(theWholeFileAsOneLongString.Split("\n"[0]) );

		CharacterFinder();
		//Iterate through each Lines in the eachLine List
	}

	void DoChoiceNode(int choiceNumber){
		newChoiceInst=Instantiate<GameObject>(choiceNodePrefab);
		childPrefab=newChoiceInst;
		childPrefab.transform.SetParent(parentPrefab.transform);

	}

	void DoChoiceRightNode(int choiceNumber){
		newChoiceRightInst=Instantiate<GameObject>(choiceNodePrefab);
		childPrefab=newChoiceRightInst;
		childPrefab.transform.SetParent(parentPrefab.transform);

	}

	void CharacterFinder(){
		grievanceOn=false;
		rightOn=false;
		for(lineCounter=0; lineCounter<eachLine.Count; lineCounter++){
			delimiter = '/';
			substrings = eachLine[lineCounter].Split(delimiter);
				Debug.Log(substring[0]);

			if(substring=="BRIANA"){
				CreatePrefab(BrianaPrefab);

			}
			if(substring=="CAPTAIN DUDINGSTON"){

				CreatePrefab(DudingstonPrefab);

			}
			if(substring=="THE SMUGGLER"){

				CreatePrefab(SmugglerPrefab);
			}
			if(substring=="THE COOK"){
				CreatePrefab(CookPrefab);
			}

			if(substring=="ANIM"){
				CreatePrefab(BrianaPrefab);
			}

			if(substring=="SOUNDEFFECT"){
				CreatePrefab(ActionPrefab);

			}
			if(substring=="THE AMERICAN"){
				CreatePrefab(AmericanPrefab);

			}
			if(substring=="Grievance # 1"){
				grievanceOn=true;
				DoChoiceNode(choiceNumberGrievance);
				CreatePrefab(BrianaPrefab);
			}
			if(substring=="Right # 1"){
				rightOn=true;
				DoChoiceRightNode(choiceNumberRight);
				CreatePrefab(BrianaPrefab);

			}

			if(substring=="Grievance # 2" || substring=="Grievance # 3" || substring=="Grievance # 4"){
				grievanceOn=false;
				choiceNumberGrievance++;
				grievanceTwoThroughFourOn=true;
				CreatePrefab(BrianaPrefab);
				if(substring=="Grievance # 4"){
					grievanceTwoThroughFourOn=false;
				}
			}

			if(substring=="Right # 2" || substring=="Right # 3" || substring=="Right # 4"){
				rightOn=false;
				choiceNumberRight++;
				rightTwoThroughFourOn=true;
				CreatePrefab(BrianaPrefab);
				if(substring=="Right # 4"){
					rightTwoThroughFourOn=false;
				}
			}



		}
	}



	void CreatePrefab(DialogueCharacter whichCharacter){

		childPrefab=Instantiate<GameObject>(prefab);

		if(substring=="ANIM"){
			Debug.Log("Hellollllll");
			childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animationToPlay=""+substrings[1];
			if(substrings[2]=="UpperDeckAnimator"){
				childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animator=upperDeckAnimator;
			}
		}

		if(substring=="SETBOOL"){
			childPrefab.ForceGetComponent<SetConditionOnEnter>().conditional=""+substrings[1];
			childPrefab.ForceGetComponent<SetConditionOnEnter>().valueToSet=Convert.ToBoolean(""+substrings[2]);
		}

		if(substring=="ENABLE"){
			childPrefab.ForceGetComponent<EnableObjectOnEnter>().thingToEnable=testThingToEnable;
			childPrefab.ForceGetComponent<EnableObjectOnEnter>().onOrOff=Convert.ToBoolean(""+substrings[2]);
		}

	
		childPrefab.transform.SetParent(parentPrefab.transform);
		if(grievanceOn || grievanceTwoThroughFourOn){
			newChoiceInst.transform.SetParent(parentPrefab.transform);
			newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].nodeChoice=childPrefab.GetComponent<DialogueNode>();
			parentPrefab.GetComponent<DialogueNode>().nextNode=newChoiceInst.GetComponent<DialogueNode>();
		}else if(rightOn || rightTwoThroughFourOn){
			newChoiceRightInst.transform.SetParent(parentPrefab.transform);
			newChoiceRightInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberRight].nodeChoice=childPrefab.GetComponent<DialogueNode>();
			parentPrefab.GetComponent<DialogueNode>().nextNode=newChoiceRightInst.GetComponent<DialogueNode>();
		}
		else{
			parentPrefab.GetComponent<DialogueNode>().nextNode=childPrefab.GetComponent<DialogueNode>();
		}
		parentPrefab=childPrefab;
		childPrefab.ForceGetComponent<DialogueNode>().character=whichCharacter;
		childPrefab.ForceGetComponent<DialogueNode>().lines= new DialogueLine[10];
		lineCounter++;
		int linesTally=0;

		while(substring!="BRIANA" && 
			substring!="CAPTAIN DUDINGSTON" && 
			substring!="SOUNDEFFECT" && 
			substring!="THE AMERICAN" && 
			substring!="CHOICES" && 
			substring!="RIGHTS" &&
			substring!="ANIM" &&
			substring!="SETBOOL" &&
			substring!="ENABLE" &&
			substring!="Grievance # 1" &&
			substring!="Grievance # 2"&&
			substring!="Grievance # 3"&&
			substring!="Grievance # 4"&&
			substring!="Right # 1"&&
			substring!="Right # 2"&&
			substring!="Right # 3"&&
			substring!="Right # 4"&&
			substring!=""){
			if(grievanceOn || grievanceTwoThroughFourOn){
				newChoiceInst.ForceGetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].responseText=eachLine[lineCounter];
				//lineCounter++;
			}else if(rightOn || rightTwoThroughFourOn){
				newChoiceRightInst.ForceGetComponent<DialogueChoiceNode>().choices[choiceNumberRight].responseText=eachLine[lineCounter];		
			}else{
				childPrefab.ForceGetComponent<DialogueNode>().lines[linesTally].text=eachLine[lineCounter];
			}
			grievanceTwoThroughFourOn=false;
			grievanceOn=false;
			rightTwoThroughFourOn=false;
			rightOn=false;

			lineCounter++;
			linesTally++;
		}
		return;
	}

}