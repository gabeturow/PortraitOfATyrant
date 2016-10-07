using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogueTree : MonoBehaviour {

	public DialogueNode root;
	public DialogueCharacter defaultCharacter;
	public DialogueNode current{get; private set;}
	public bool zoomIn=false;
	public float howMuchZoom=3;
	public float howMuchRightMove = .5f;
	public bool RightSideConvo=false;
	CharacterAnimation characterNew;

	void Awake(){
		characterNew = DialogueViewer.main.characterOne.GetComponent<CharacterAnimation>();
	}

	public void Init(){
		current = root;
		current.Init();
	}

	void Update(){

	}

	public bool IsComplete{
		get{ return (current == null || root == null || (current.nextNode == null && current.IsComplete)); }
	}

	public DialogueLine CurrentMessage{
		get{ return current.CurrentMessage; }
	}

	public bool IsBlocked = false;

	public void AdvanceDialogue(){
		DialogueViewer.main.ZoomCamera=zoomIn;
		DialogueViewer.main.moveright=RightSideConvo;
		characterNew.TurnCharacterLeft(!RightSideConvo);
			
		if (current.IsComplete){
			current = current.nextNode;
			current.Init();
			this.IsBlocked = true;
			current.TryBlock(OnUnblock);
			current.DoBlock();
		}
		current.AdvanceMessage();
	}


	void OnUnblock(){
		this.IsBlocked = false;
	}

	public DialogueChoice[] GetChoices(){
		var choiceNode = current as DialogueChoiceNode;
		if (choiceNode != null){
			return choiceNode.choices;
		}
		return null;
	}


#if UNITY_EDITOR
	/*
	void OnDrawGizmos(){
		if (root == null){ return; }
		Handles.color = Color.red;
		Gizmos.color = Color.white;
		GUI.color = Color.white;
		Vector3 sceneCamPos = SceneView.currentDrawingSceneView.camera.transform.position;
		float distCam = SceneView.currentDrawingSceneView.camera.transform.TransformPoint(root.transform.position).magnitude;

		Gizmos.DrawWireCube(root.transform.position, Vector3.one * 1.5f);
		Gizmos.DrawWireCube(root.transform.position, Vector3.one * 2.0f);
		Vector3 textOffset = Vector3.Cross(sceneCamPos - root.transform.position, Vector3.up).normalized;
		Vector3 textPos = root.transform.position + textOffset;
	}
*/
#endif



}
