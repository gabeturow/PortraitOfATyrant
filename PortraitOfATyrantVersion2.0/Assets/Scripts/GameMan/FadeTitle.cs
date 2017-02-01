using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FadeTitle : MonoBehaviour {
	public int delay=3;
	public bool gofade=false;
	public Text whatText;

	// Use this for initialization
	void Start () {

		FadeGo();
	}
	
	// Update is called once per frame
	void Update () {
		if(gofade){
			Color color = whatText.color;
			color.a -= 0.001f;
			whatText.color = color;
		}
	}

	IEnumerator FadeGo(){
		yield return new WaitForSeconds(delay);
		gofade=true;
	}
}
