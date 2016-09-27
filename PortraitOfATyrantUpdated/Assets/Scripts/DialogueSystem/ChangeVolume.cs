using UnityEngine;
using System.Collections;

public class ChangeVolume : MonoBehaviour {

	public AudioSource whichAudioSource;

	void ChangeVolumeUpperDecks(){
		if(RoomMan.main.current != null){
			//Debug.Log (RoomMan.main.current.name=="CaptainsQuarters(Clone)");

			if(RoomMan.main.current.name=="UpperDeck 1(Clone)"){
					whichAudioSource.volume=.1f;
			}else if(RoomMan.main.current.name=="UpperDeck 2(Clone)"){
					whichAudioSource.volume=.2f;
			}else if(RoomMan.main.current.name=="UpperDeck 3(Clone)"){
					whichAudioSource.volume=.3f;
			}else if(RoomMan.main.current.name=="Hold(Clone)"){
				whichAudioSource.volume=0;
			}

		}
	}

	void Update () {

		ChangeVolumeUpperDecks();

	}
}