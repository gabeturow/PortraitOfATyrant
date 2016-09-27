using UnityEngine;
using System.Collections;

public class MultiInteractionModule : InteractionModule {

	[TextArea(1, 5)]
	string notes;

	[SerializeField]
	Transform[] moduleFolders;

	public override void OnInteract ()
	{
		for(int i = 0; i < moduleFolders.Length; i++){
			var folder = moduleFolders[i];
			if (CheckInteract(folder)){
				Interact(folder);
				return;
			}
		}
	}

	bool CheckInteract(Transform folder){
		var childCount = folder.childCount;
		for(int i = 0; i < childCount; i++){
			bool canInteract = true;
			var module = folder.GetChild(i).GetComponent<InteractionModule>();
			if (module != null){
				canInteract = canInteract && module.CheckInteract();
			}
		}
		return false;
	}

	void Interact(Transform folder){
		var childCount = folder.childCount;
		for(int i = 0; i < childCount; i++){
			var module = folder.GetChild(i).GetComponent<InteractionModule>();
			if (module != null){
				module.OnInteract();
			}
		}
	}




}
