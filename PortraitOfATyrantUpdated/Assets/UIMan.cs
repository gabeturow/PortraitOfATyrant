using UnityEngine;
using System.Collections;

public class UIMan : MonoBehaviour {

	public static UIMan main 							{ get; private set; }
	public static bool declarationStatus=false;
	public static BigItemViewer bigItemViewer			{ get; private set; }
	public static DeclarationViewer declarationViewer 	{ get; private set; }

	public bool IsPanelActive(){
		return bigItemViewer.active || declarationViewer.active;
	}

	void Awake(){
		if (!EnsureSingleton()) {
			return;
		}
		bigItemViewer = GetComponentInChildren<BigItemViewer>();
		declarationViewer = GetComponentInChildren<DeclarationViewer>();
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
