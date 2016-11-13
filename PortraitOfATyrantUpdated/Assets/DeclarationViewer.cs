using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeclarationViewer : PopUpViewer {

	[SerializeField]
	Image[] passages;

	public void AnimateReveal(int passageNumber){
		var passage = passages[passageNumber];
		passage.CrossFadeAlpha(1f, 1f, true);
	}

}
