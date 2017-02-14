using UnityEngine;
using System.Collections;

public class EnableObjectOnEnter :	OnEnterNode {

		[SerializeField]
		public GameObject thingToEnable;

		[SerializeField]
		public bool onOrOff;

		public override void OnEnter (DialogueNode node){
		thingToEnable.SetActive(onOrOff);
		}
}