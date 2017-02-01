using UnityEngine;
using System.Collections;

public class CanvasColorSpring : ColorSpring {

	new public CanvasRenderer renderer;

	protected override void Awake(){
		if (renderer == null)
			renderer = GetComponent<CanvasRenderer>();
		base.Awake();
	}

	void Update () {
		SetCurrentRenderColor(Utilities.SimpleHarmonicMotion((Vector4)GetCurrentRenderColor(), (Vector4)target, ref velocity, strength, dampingRatio));
	}

	public override Color GetCurrentRenderColor(){
		return this.renderer.GetColor();
	}


	protected override void SetCurrentRenderColorInternal(Color color){
		this.renderer.SetColor(color);
	}

}
