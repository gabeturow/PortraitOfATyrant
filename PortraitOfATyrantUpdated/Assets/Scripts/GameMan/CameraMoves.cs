using UnityEngine;
using System.Collections;

public class CameraMoves : MonoBehaviour {

	public static CameraMoves main;
		Camera camera;
		public float cameraX = 0f;
		public float cameraY1 = 1.0f;
		public float cameraZoom = 0f;
		public GameObject cameraObj;
		public bool cameraGo=false;
	public float howMuchMoveRight = .5f;
	public bool ZoomCamera=true;
	public bool moveright=false;
	public DialogueViewer dialogueViewer;
	public Vector3 cameraStartPointReceiver;


	void Start(){
		camera=GetComponent<Camera>();
	}
		
	public void ResetCameraToNewCoordinates(){

		transform.localPosition=cameraStartPointReceiver;
	}

	void Update(){


		
		if(ZoomCamera){
			camera.fieldOfView=cameraZoom;
			transform.localPosition= new Vector3(cameraX,cameraY1,-100f);


			if(moveright){
				if(cameraGo){
					if(cameraX<dialogueViewer.currentDialogue.howMuchRightMove){
						cameraX+=.01f;
					}
				}else if(cameraX>0f){
					cameraX-=.02f;
				}
			}else if(cameraGo){
				if(cameraX>-2f){
					cameraX-=.01f;
				}
			}else if(cameraX<0f){
				cameraX+=.02f;
			}

		////	Move Y
			if(cameraGo){
				if(cameraY1<dialogueViewer.currentDialogue.howMuchMoveY){
					cameraY1+=.01f;
				}
			}else if(cameraY1>1f){
				cameraY1-=.02f;
			}

			float baseline=5f;
			////  Move Z - Field of View
			if(cameraGo){
				if(cameraZoom<(baseline+.001) && cameraZoom>=dialogueViewer.currentDialogue.howMuchZoom){
					cameraZoom-=.01f;
				}else if(baseline<dialogueViewer.currentDialogue.howMuchZoom && cameraZoom<dialogueViewer.currentDialogue.howMuchZoom){
					cameraZoom+=.01f;
				}
			}else{
						if(cameraZoom<8f){
					cameraZoom+=.02f;
					}
						if(cameraZoom>8f){
							cameraZoom-=.02f;
						}

			}
		}
	}

}
