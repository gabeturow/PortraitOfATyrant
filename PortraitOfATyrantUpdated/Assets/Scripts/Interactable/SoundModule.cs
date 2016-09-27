using UnityEngine;
using System.Collections;

public class SoundModule : InteractionModule {

	[SerializeField]
	AudioClip[] clips;
	AudioClip clipToPlay{get{ return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null; } }
	AudioSource source;

	public override void OnInteract ()
	{

		if (clipToPlay != null){
			if (source == null) source = gameObject.ForceGetComponent<AudioSource>();
		//	source.priority=1;
			AudioSource.PlayClipAtPoint(clipToPlay, transform.position);

			//return source;
		}
	}

}
