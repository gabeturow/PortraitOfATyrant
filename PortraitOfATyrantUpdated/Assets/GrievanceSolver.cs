using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

[System.Serializable]
public class GrievancePuzzle{
	public GrievancePuzzle(InventoryObject[] correct){
		this.correct = correct;
	}
	public InventoryObject[] correct;
	public GrievancePuzzle Clone(){
		return new GrievancePuzzle(correct.Clone() as InventoryObject[]);
	}
}

public class GrievanceSolver : MonoBehaviour {
	public static GrievanceSolver main;

	CanvasGroupFader fader;

	GrievancePuzzle puzzle;

	[SerializeField]
	GrievanceSlot[] rightsSlots;

	[SerializeField]
	GrievanceSlot[] grievanceSlots;

	[SerializeField]
	Button submitButton;

	System.Action<bool> onFinish;

	public void StartPuzzle(GrievancePuzzle puzzle, System.Action<bool> onFinish){
		this.puzzle = puzzle.Clone();
		this.onFinish = onFinish;
		ClearSlots();
		fader.displaying = true;
	}

	void ClearSlots(){
		for(int i = 0; i < grievanceSlots.Length; i++){
			grievanceSlots[i].ClearSlot();
		}
		for(int i = 0; i < rightsSlots.Length; i++){
			rightsSlots[i].ClearSlot();
		}
	}

	public void TrySolve(){

		bool solved = true;
		for(int i = 0; i < grievanceSlots.Length; i++){
			var grievance = grievanceSlots[i].slotted;
			solved = solved && ContainsGrievance(grievance);
			if (solved){
				RemoveFromPuzzle(grievance);
			}
			else{
				break;
			}
		}

		if (onFinish != null){
			onFinish(solved);
		}

		fader.displaying = false;
	}

	bool ContainsGrievance(InventoryObject item){
		return puzzle.correct.Contains(item);
	}

	void RemoveFromPuzzle(InventoryObject item){
		puzzle.correct = puzzle.correct.Where(i=>{return i != item;}).ToArray();
	}


	public void Hide(){
		fader.displaying = false;
	}

	void Awake(){
		main = this;
		fader = gameObject.ForceGetComponent<CanvasGroupFader>();
	}

}
