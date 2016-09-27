using UnityEngine;
using System.Collections;

public abstract class ColorSpring : MonoBehaviour {
	
	public Color currentColor;
	public bool useInitialColor = false;
	public Color initialColor = new Color(1,1,1,0);
	public Color target;
	public float scalar = 1;
	public Vector4 velocity;
	public float strength = 2;
	public float dampingRatio = 1;

	protected virtual void Awake(){
		if (useInitialColor)
			SetCurrentRenderColor(initialColor);
	}
	
	void Update () {
		SetCurrentRenderColor(Utilities.SimpleHarmonicMotion((Vector4)GetCurrentRenderColor(), (Vector4)target, ref velocity, strength, dampingRatio));
	}
	
	public abstract Color GetCurrentRenderColor();
	
	
	public virtual void SetCurrentRenderColor(Color color){
		SetCurrentRenderColorInternal(color * scalar);
	}

	protected abstract void SetCurrentRenderColorInternal(Color color);



	
}
