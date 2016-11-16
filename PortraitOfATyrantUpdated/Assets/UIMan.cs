using UnityEngine;
using System.Collections;

public class UIMan : MonoBehaviour {

	public static UIMan main 							{ get; private set; }

	//subsystems
	public static BigItemViewer bigItemViewer			{ get; private set; }
	public static DocumentViewer declaration			{ get; private set; }
	public static DocumentViewer billOfRights			{ get; private set; }


	public bool IsPanelActive(){
		return bigItemViewer.active || declaration.active || billOfRights.active;
	}

	void Awake(){
		if (!EnsureSingleton()) {
			return;
		}

		InstantiateSubsystems();
	}


	internal void InstantiateSubsystems(){
		bigItemViewer = CreateSubsystem<BigItemViewer>("BigItemViewer");
		declaration = CreateSubsystem<DocumentViewer>("DeclarationViewer");
		billOfRights = CreateSubsystem<DocumentViewer>("BillOfRightsViewer");
	}

	internal bool EnsureSingleton(){
		if (main != null) {
			Debug.LogError("UIMan already exists!");
			Destroy(this.gameObject);
			return false;
		}
		main = this;
		return true;
	}

	internal T CreateSubsystem<T>(string prefabName) where T : Component{
		var subsystem = PrefabManager.Instantiate(prefabName).GetComponent<T>();
		SetParentOfSubsystem(subsystem);
		return subsystem;
	}

	internal void SetParentOfSubsystem(Component subsystem){
		var rectTransform = subsystem.GetComponent<RectTransform>();
		rectTransform.SetParent(this.transform);
		rectTransform.localPosition = Vector3.zero;
		rectTransform.localRotation = Quaternion.identity;
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		rectTransform.pivot = new Vector2(.5f, .5f);
	}

}
