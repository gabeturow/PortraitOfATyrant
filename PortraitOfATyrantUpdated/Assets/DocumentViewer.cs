﻿using UnityEngine;
using System.Collections;

public class DocumentViewer : PopUpViewer {

	[SerializeField]
	AnimatedPassage[] passages;


	protected override void Awake (){
		base.Awake();
		HideAllPassages();
	}


	public void AnimateReveal(int passageNumber){
		UIMan.main.TurnOnDeclaration();
		//animate in our passage we selected
		var passage = passages[passageNumber];
		passage.AnimateIn();
	


		//fade in half opacity declaration
		//fade in full opacity declaration img after seconds
		SetHalfOpacityDeclaration();
		Invoke("FadeInDeclaration", 2f);

		//don't allow user to dismiss this panel until seconds pass
		DisablePanelHiding();
		Invoke("EnablePanelHiding", 3f);
	}

	void SetHalfOpacityDeclaration(){
		itemImage.CrossFadeAlpha(.5f, 1f, true);
	}

	void FadeInDeclaration(){
		itemImage.CrossFadeAlpha(1f, 1f, true);
	}

	void DisablePanelHiding(){
		canHide = false;
	}

	void EnablePanelHiding(){
		canHide = true;
	}

	void HideAllPassages(){
		for (int i = 0; i < passages.Length; i++) {
			passages[i].Hide();
		}
	}
}
