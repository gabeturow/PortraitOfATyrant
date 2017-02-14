using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WriteIn : MonoBehaviour {

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

	public Sprite prosperity;
	public Sprite liberty;
	public Sprite justice;
	public Sprite peace;


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
					

			if(substrings[0]=="BRIANA"){
					CreatePrefab(BrianaPrefab);
			
				}
			if(eachLine[lineCounter]=="CAPTAIN DUDINGSTON"){

					CreatePrefab(DudingstonPrefab);

				}
			if(eachLine[lineCounter]=="THE SMUGGLER"){

					CreatePrefab(SmugglerPrefab);
				}
				if(eachLine[lineCounter]=="THE COOK"){
					CreatePrefab(CookPrefab);
				}

			if(substrings[0]=="CON"){
				CreatePrefab(CookPrefab);
			}

				if(eachLine[lineCounter]=="SOUNDEFFECT"){
					CreatePrefab(ActionPrefab);

				}
				if(eachLine[lineCounter]=="THE AMERICAN"){
					CreatePrefab(AmericanPrefab);

				}
			if(eachLine[lineCounter]=="Grievance # 1"){
					grievanceOn=true;
					DoChoiceNode(choiceNumberGrievance);
					CreatePrefab(BrianaPrefab);

				}
				if(eachLine[lineCounter]=="Right # 1"){
					rightOn=true;
					DoChoiceRightNode(choiceNumberRight);
					CreatePrefab(BrianaPrefab);

				}

				if(eachLine[lineCounter]=="Grievance # 2" || eachLine[lineCounter]=="Grievance # 3" || eachLine[lineCounter]=="Grievance # 4"){
					grievanceOn=false;
					choiceNumberGrievance++;
					grievanceTwoThroughFourOn=true;
					CreatePrefab(BrianaPrefab);
					if(eachLine[lineCounter]=="Grievance # 4"){
						grievanceTwoThroughFourOn=false;
					}
				}

				if(eachLine[lineCounter]=="Right # 2" || eachLine[lineCounter]=="Right # 3" || eachLine[lineCounter]=="Right # 4"){
					rightOn=false;
					choiceNumberRight++;
					rightTwoThroughFourOn=true;
					CreatePrefab(BrianaPrefab);
					if(eachLine[lineCounter]=="Right # 4"){
						rightTwoThroughFourOn=false;
					}
				}



			}
		}



		void CreatePrefab(DialogueCharacter whichCharacter){
		if(substrings[0]=="CON"){
			for(int x=0;x<substrings.Length;x++){
				Debug.Log(substrings[x]);
				if(substrings[x]=="SETBOOL"){
					childPrefab.ForceGetComponent<SetConditionOnEnter>().conditional=""+substrings[2];
					childPrefab.ForceGetComponent<SetConditionOnEnter>().valueToSet=Convert.ToBoolean(""+substrings[3]);
				}
				if(substrings[x]=="ANIM"){
					
					childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animationToPlay=""+substrings[2];
					if(substrings[3]=="UpperDeckAnimator"){
						childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animator=upperDeckAnimator;
					}
				}
				if(substrings[x]=="ENABLE"){
					childPrefab.ForceGetComponent<EnableObjectOnEnter>().thingToEnable=testThingToEnable;
					childPrefab.ForceGetComponent<EnableObjectOnEnter>().onOrOff=Convert.ToBoolean(""+substrings[3]);
				}
			}
		}
			childPrefab=Instantiate<GameObject>(prefab);
			childPrefab.transform.SetParent(parentPrefab.transform);
			if(grievanceOn || grievanceTwoThroughFourOn){
				newChoiceInst.transform.SetParent(parentPrefab.transform);
			newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].nodeChoice=childPrefab.GetComponent<DialogueNode>();
			newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].responseText=substrings[1];
			newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].label=substrings[3];
			if(substrings[3]=="PROSPERITY"){
				newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=prosperity;
			}
			if(substrings[3]=="LIBERTY"){
				newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=liberty;
			}
			if(substrings[3]=="PEACE"){
				newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=peace;
			}
			if(substrings[3]=="JUSTICE"){
				newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=justice;
			}

			//Set choice node as next node on previous node
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

			while(eachLine[lineCounter]!="BRIANA" && 
				eachLine[lineCounter]!="CAPTAIN DUDINGSTON" && 
				eachLine[lineCounter]!="SOUNDEFFECT" && 
				eachLine[lineCounter]!="THE AMERICAN" && 
				eachLine[lineCounter]!="CHOICES" && 
				eachLine[lineCounter]!="RIGHTS" &&
				eachLine[lineCounter]!="Grievance # 1" &&
				eachLine[lineCounter]!="Grievance # 2"&&
				eachLine[lineCounter]!="Grievance # 3"&&
				eachLine[lineCounter]!="Grievance # 4"&&
				eachLine[lineCounter]!="Right # 1"&&
				eachLine[lineCounter]!="Right # 2"&&
				eachLine[lineCounter]!="Right # 3"&&
				eachLine[lineCounter]!="Right # 4"&&
				eachLine[lineCounter]!=""){
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
