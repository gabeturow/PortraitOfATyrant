using UnityEngine;
using System.Collections;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct DialogueLine{
	[TextArea(5, 3)]
	public string text;
	public AudioClip audioClip;
}

public class DialogueNode : MonoBehaviour {

	public string currentLineNow;
	public static DialogueNode main;
	public DialogueCharacter character;
	public DialogueNode nextNode;
	public InventoryObject inventoryObj;

	protected OnEnterNode[] onEnter;

	public static Sprite rightsImageMenu;
	public static string rightsLabelMenu;

	public DialogueLine[] lines;

	public int currentMessageIndex{get; private set;}

	private GUIStyle labelStyle = new GUIStyle();
	private StringBuilder sb = new StringBuilder();

	//ITEM RELATED


	/// ///////////////


	System.Action onUnblock;

	public virtual void Init(){
		onEnter = this.GetComponents<OnEnterNode>();
		for (int i = 0; i < onEnter.Length; i++) {
			onEnter[i].OnEnter(this);
		}
		ReceiveItem(inventoryObj);
		this.currentMessageIndex = -1;
		//GameMan.main.conditionals.SetValue(condition, valueToSet);

	}

	public bool CanAdvance(){
		
		if (onEnter != null) {
			for (int i = 0; i < onEnter.Length; i++) {
				if (!onEnter[i].IsFinished())
					return false;
			}
		}
		return true;
	}

	public void ReceiveItem(InventoryObject inventory){

	
		if (inventoryObj != null){
			InventoryMan.main.Add(inventoryObj);
			//pick up by adding this object to the internal hashset
		//	pickups.Add(this.Identifier);
		}
	}

	public virtual DialogueLine CurrentMessage{ 
		

		get{ return lines[currentMessageIndex];
		} 
	}

	public virtual bool IsComplete{ 
		get{ return currentMessageIndex >= lines.Length-1; } 
	}

	public virtual string this[int index]{ 
		get{ return lines[index].text;  } 
		set{ lines[index].text = value; } 
	}

	public virtual void AdvanceMessage(){
		currentMessageIndex = Mathf.Clamp(currentMessageIndex + 1, 0, lines.Length);
	}

	public void TryBlock(System.Action onUnblock){
		this.onUnblock = onUnblock;
	}
	public virtual void DoBlock(){
		UnBlock();
	}

	public virtual void UnBlock(){
		if (this.onUnblock != null){
			this.onUnblock();
		}
	}



	//editor ui
#if UNITY_EDITOR
	void OnDrawGizmos(){
		Handles.color = Color.red;
		Gizmos.color = Color.yellow;
		GUI.color = Color.white;
		Vector3 sceneCamPos = SceneView.currentDrawingSceneView.camera.transform.position;
		float distCam = SceneView.currentDrawingSceneView.camera.transform.TransformPoint(transform.position).magnitude;
		labelStyle.fontSize = Mathf.RoundToInt(20/distCam) * 6;
		labelStyle.normal.textColor = Color.white;
		labelStyle.fixedWidth = ((labelStyle.fontSize * 10));
		labelStyle.wordWrap = true;
		labelStyle.alignment = TextAnchor.MiddleLeft;

		Gizmos.DrawWireCube(transform.position, Vector3.one);
		Vector3 textOffset = Vector3.Cross(sceneCamPos - transform.position, Vector3.up).normalized;
		Vector3 textPos = transform.position + textOffset;
		Handles.Label(textPos, GetString(), labelStyle);
		DrawConnectors();
	}

	protected virtual void DrawConnectors(){
		if (nextNode != null){
			DrawArrow.Connector(transform.position, nextNode.transform.position - transform.position);
		}
	}

	protected virtual string GetString(){
		sb.Length = 0;
		sb.AppendLine("<color=grey>" + this.gameObject.name + "</color>\n");
		for(int i = 0; i < lines.Length; i++){
			sb.AppendLine(lines[i].text);

			//WriteHere.main.writeToString+=lines[i].text;

		}
		return sb.ToString();

	}
#endif
}
