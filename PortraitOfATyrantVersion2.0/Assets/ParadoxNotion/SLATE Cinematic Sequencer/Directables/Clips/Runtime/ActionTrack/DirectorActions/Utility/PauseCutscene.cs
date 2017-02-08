namespace Slate.ActionClips{

	[Category("Utility")]
	[Description("Pauses the Cutscene. It's up to other scripts to resume it.")]
	public class PauseCutscene : DirectorActionClip {
		protected override void OnEnter(){
			if (UnityEngine.Application.isPlaying){
				(root as Cutscene).Pause();
			}
		}
	}
}