using UnityEngine;

namespace Slate{

	[ExecuteInEditMode]
	public class OnGUIEventDispatcher : MonoBehaviour {
		public event System.Action onGUI;
		void OnGUI(){
			if (onGUI != null){
				onGUI();
			}
		}
	}
}