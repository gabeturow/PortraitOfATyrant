using UnityEngine;
using System.Collections;

/// <summary>
/// Recursive canvas color spring. Don't add more objects to this guy.
/// </summary>
public class RecursiveCanvasColorSpring : ColorSpring {

	Color color = Color.white;
	new public CanvasRenderer[] renders;

	protected override void Awake(){
		renders = GetComponentsInChildren<CanvasRenderer>();
		base.Awake();
	}

	void Update () {
		SetCurrentRenderColor(Utilities.SimpleHarmonicMotion((Vector4)GetCurrentRenderColor(), (Vector4)target, ref velocity, strength, dampingRatio));
	}

	public override Color GetCurrentRenderColor(){
		return color;
	}


	protected override void SetCurrentRenderColorInternal(Color color){
		for(int i = 0; i < renders.Length; i++){
			renders[i].SetColor(color);
		}
		this.color = color;
	}

}
