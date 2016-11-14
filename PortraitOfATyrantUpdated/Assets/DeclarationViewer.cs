using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeclarationViewer : PopUpViewer {

	[SerializeField]
	Image[] passages;

	protected override void Awake ()
	{
		base.Awake();
		for (int i = 0; i < passages.Length; i++) {
			passages[i].CrossFadeAlpha(0, 0f, true);
		}
	}

	public void AnimateReveal(int passageNumber){
		var passage = passages[passageNumber];
		passage.CrossFadeAlpha(1f, 2f, true);
	}

}
