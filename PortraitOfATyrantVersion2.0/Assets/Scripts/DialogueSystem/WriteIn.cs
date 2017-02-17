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

	public string newTag;

	public AudioClip silence;


	public Animator upperDeckAnimator;
	public Animator AmericansRunAway;
	public Animator CaptainNode;


	void Start(){
		
		theWholeFileAsOneLongString = dictionaryTextFile.text;

		//Split it into Lines
		eachLine.AddRange(theWholeFileAsOneLongString.Split("\n"[0]) );

		CharacterFinder();
		//Iterate through each Lines in the eachLine List

		GetComponent<DialogueTree>().root=startNode.GetComponent<DialogueNode>();



	}

	void DoChoiceNode(int choiceNumber){
		newChoiceInst=Instantiate<GameObject>(choiceNodePrefab);
		childPrefab=newChoiceInst;
		childPrefab.transform.SetParent(parentPrefab.transform);
	}

	void ProsperityTranslate(){
		for(int x=0;x<substrings.Length;x++){
			if(newChoiceInst.GetComponent<DialogueChoiceNode>().choices.Length>choiceNumberGrievance){
				if(substrings.Length>1){
					newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].responseText=substrings[0];
					newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].label=substrings[1];
					if(substrings[x]=="PROSPERITY"){
							newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=prosperity;
					}
					if(substrings[x]=="LIBERTY"){
							newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=liberty;
					}
					if(substrings[x]=="PEACE"){
							newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=peace;
					}
					if(substrings[x]=="JUSTICE"){
							newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].grievanceImage=justice;
					}
				}
			}
		}

	}

	void ProsperityTranslateRights(){
		for(int x=0;x<substrings.Length;x++){
			if(newChoiceRightInst.GetComponent<DialogueChoiceNode>().choices.Length>choiceNumberRight){
			if(substrings.Length>1){
				newChoiceRightInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberRight].responseText=substrings[0];
				newChoiceRightInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberRight].label=substrings[1];
				newChoiceRightInst.GetComponent<DialogueChoiceNode>().rightsLabel=substrings[1];
				if(substrings[x]=="PROSPERITY"){
					newChoiceRightInst.GetComponent<DialogueChoiceNode>().rightsImage=prosperity;
				}
				if(substrings[x]=="LIBERTY"){
					newChoiceRightInst.GetComponent<DialogueChoiceNode>().rightsImage=liberty;
				}
				if(substrings[x]=="PEACE"){
					newChoiceRightInst.GetComponent<DialogueChoiceNode>().rightsImage=peace;
				}
				if(substrings[x]=="JUSTICE"){
					newChoiceRightInst.GetComponent<DialogueChoiceNode>().rightsImage=justice;
				}
			}
			}
		}

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

			//SET TAG
			newTag=substrings[0];

			if(substrings[0]=="CON"){
				CreatePrefab(CookPrefab);
			}

			if(substrings[0]=="END"){
				CreatePrefab(BrianaPrefab);
			}

			if(substrings[0]=="CHOICES"){
				CreatePrefab(BrianaPrefab);
			}

			if(substrings[0]=="RIGHTS"){
				CreatePrefab(BrianaPrefab);
			}

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
				rightOn=false;
				choiceNumberGrievance++;
				grievanceTwoThroughFourOn=true;
				CreatePrefab(BrianaPrefab);

			}

			if(eachLine[lineCounter]=="Right # 2" || eachLine[lineCounter]=="Right # 3" || eachLine[lineCounter]=="Right # 4"){
				rightOn=false;
				grievanceOn=false;
				grievanceTwoThroughFourOn=false;
				rightTwoThroughFourOn=true;
				choiceNumberRight++;

				CreatePrefab(BrianaPrefab);
			}



		}
	}


	void CreatePrefab(DialogueCharacter whichCharacter){

		if(parentPrefab.ForceGetComponent<DialogueNode>().lines[0].text==""){
			parentPrefab.ForceGetComponent<DialogueNode>().lines[0].audioClip=silence;
		}

		if(substrings[0]=="CON"){
			for(int x=0;x<substrings.Length;x++){
				
				if(substrings[x]=="SETBOOL"){
					childPrefab.ForceGetComponent<SetConditionOnEnter>().conditional=""+substrings[x+1];
					childPrefab.ForceGetComponent<SetConditionOnEnter>().valueToSet=Convert.ToBoolean(""+substrings[x+2]);
				}
				if(substrings[x]=="ANIM"){

					childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animationToPlay=""+substrings[x+1];
					if(substrings[x+2]=="UpperDeckAnimator"){
						childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animator=upperDeckAnimator;
					}
					if(substrings[x+2]=="AmericansRunAway"){
						childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animator=AmericansRunAway;
					}
					if(substrings[x+2]=="Captain"){
						childPrefab.ForceGetComponent<AnimationDialogueOnEnter>().animator=CaptainNode;
					}
				}
				if(substrings[x]=="ENABLE"){
					childPrefab.ForceGetComponent<EnableObjectOnEnter>().thingToEnable=testThingToEnable;
					childPrefab.ForceGetComponent<EnableObjectOnEnter>().onOrOff=Convert.ToBoolean(""+substrings[x+2]);
				}


				if(substrings[x]=="PLAYDEC"){
					childPrefab.ForceGetComponent<PlayDeclarationAnimOnEnter>().passageNumber=Convert.ToInt16(substrings[x+2]);
					var animEnum=System.Enum.Parse(typeof(PlayDeclarationAnimOnEnter.Document), ""+substrings[x+1], true);
					childPrefab.ForceGetComponent<PlayDeclarationAnimOnEnter>().document=(PlayDeclarationAnimOnEnter.Document)animEnum;
						
						//parse the string into the animation enum
						
				}

			}
		}
		childPrefab=Instantiate<GameObject>(prefab);
		childPrefab.transform.SetParent(parentPrefab.transform);

		if(grievanceOn || grievanceTwoThroughFourOn){
			
			newChoiceInst.transform.SetParent(parentPrefab.transform);
			newChoiceInst.GetComponent<DialogueChoiceNode>().conditionalState="GRIEVANCE";
			newChoiceInst.GetComponent<DialogueChoiceNode>().conditionalBool=true;
			if(newChoiceInst.GetComponent<DialogueChoiceNode>().choices.Length>choiceNumberGrievance){
			newChoiceInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberGrievance].nodeChoice=childPrefab.GetComponent<DialogueNode>();
			}
				if(eachLine[lineCounter]=="Grievance # 4"){
				Debug.Log(grievanceTwoThroughFourOn);
			}
			
				
			//Set choice node as next node on previous node
			parentPrefab.GetComponent<DialogueNode>().nextNode=newChoiceInst.GetComponent<DialogueNode>();

		}else if(rightOn || rightTwoThroughFourOn){
			newChoiceRightInst.GetComponent<DialogueChoiceNode>().conditionalState="RIGHTS";
			newChoiceRightInst.GetComponent<DialogueChoiceNode>().conditionalBool=true;
			//newChoiceRightInst.transform.SetParent(parentPrefab.transform);
			if(newChoiceRightInst.GetComponent<DialogueChoiceNode>().choices.Length>choiceNumberRight){
			newChoiceRightInst.GetComponent<DialogueChoiceNode>().choices[choiceNumberRight].nodeChoice=childPrefab.GetComponent<DialogueNode>();
			}
			parentPrefab.GetComponent<DialogueNode>().nextNode=newChoiceRightInst.GetComponent<DialogueNode>();
		}
		else{
			if(newTag=="END"){
				parentPrefab.GetComponent<DialogueNode>().nextNode=null;
			}
			if(newTag=="CHOICES"){
				parentPrefab.GetComponent<DialogueNode>().nextNode=newChoiceInst.GetComponent<DialogueChoiceNode>();
			}
			if(newTag=="RIGHTS"){
				parentPrefab.GetComponent<DialogueNode>().nextNode=newChoiceRightInst.GetComponent<DialogueChoiceNode>();
			}
			if(newTag!="CHOICES" && newTag!="END" && newTag!="RIGHTS"){
				parentPrefab.GetComponent<DialogueNode>().nextNode=childPrefab.GetComponent<DialogueNode>();
			}
				
		}
		parentPrefab=childPrefab;
		childPrefab.ForceGetComponent<DialogueNode>().character=whichCharacter;
	//childPrefab.ForceGetComponent<DialogueNode>().lines= new DialogueLine[10];
		lineCounter++;
		substrings = eachLine[lineCounter].Split(delimiter);

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
		
					ProsperityTranslate();
		
			}else if(rightOn || rightTwoThroughFourOn){
			
				ProsperityTranslateRights();
			}else{
				DialogueLine[] tempDiag=childPrefab.ForceGetComponent<DialogueNode>().lines;
				childPrefab.ForceGetComponent<DialogueNode>().lines= new DialogueLine[linesTally+1];
				for(int x=0;x<tempDiag.Length;x++){
					childPrefab.ForceGetComponent<DialogueNode>().lines[x]=tempDiag[x];
				}
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
