using UnityEngine;
using System.Collections;

public class ToastSendAudioOnInteract : InteractionModule {
		
	[SerializeField]
	AudioClip clipSolo;
	public AudioSource source;


		public override void OnInteract ()
		{

		if (clipSolo != null){
			source.clip=clipSolo;
			source.Play();
			}

		}
}