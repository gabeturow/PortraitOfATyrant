using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SamCinema;

public delegate void SamActionDelegate();
public class SamCinemaManager : MonoBehaviour {
	public static SamCinemaManager main;

	private List<Vignette> vignettes;
	private List<Vignette> toRemove;

	void Awake(){
		main = this;
		vignettes = new List<Vignette>();
		toRemove = new List<Vignette>();
	}


	void Update () {
		UpdateVignettes();
	}


	void UpdateVignettes(){
		for(int i = 0; i < vignettes.Count; i++){
			if (toRemove.Count < 1 || !toRemove.Contains(vignettes[i])){
				vignettes[i].Update();
				if (vignettes[i].IsFinished){
					toRemove.Add(vignettes[i]);
				}
			}
		}
		RemoveDoneVignettes();
	}

	void RemoveDoneVignettes(){
		if (toRemove.Count < 1) return;
		for(int i = 0; i < toRemove.Count; i++){
			vignettes.Remove(toRemove[i]);
			toRemove[i] = null;
		}
		toRemove.Clear();
	}


	public void AddVignette(Vignette vignette){
		if (vignettes.Contains(vignette)) {Debug.LogError("Cannot add Vignette. Already exists."); return;}
		vignettes.Add(vignette);
		vignette.InternalPlay();
	}


	public void RemoveVignette(Vignette vignette){
		toRemove.Add(vignette);
	}

	public bool IsRunning(Vignette v){
		return vignettes.Contains(v);
	}


}
