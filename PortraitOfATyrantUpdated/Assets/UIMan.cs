using UnityEngine;
using System.Collections;

public class UIMan : MonoBehaviour {

	public static UIMan main 							{ get; private set; }
	public bool declarationStatus=false;
	public static BigItemViewer bigItemViewer			{ get; private set; }
	public static DocumentViewer documentViewer 	{ get; private set; }
	public GameObject declarationObject;
	public GameObject rightsObject;


	public bool IsPanelActive(){
		return bigItemViewer.active || documentViewer.active;
	}

	void Awake(){
		if (!EnsureSingleton()) {
			return;
		}
		bigItemViewer = GetComponentInChildren<BigItemViewer>();
		documentViewer = GetComponentInChildren<DocumentViewer>();

	}

	public void TurnOnDeclaration(){
		declarationObject.SetActive(declarationStatus);
		rightsObject.SetActive(!declarationStatus);
	}

	bool EnsureSingleton(){
		if (main != null) {
			Debug.LogError("UIMan already exists!");
			Destroy(this.gameObject);
			return false;
		}
		main = this;
		return true;
	}

}
