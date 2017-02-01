using UnityEngine;
using System.Collections;

namespace SamCinema{
	//an action is one unit of event.
	//it can be attached to an actor, gameobject, or fire off functions.
	//main point of it is to time events.

	public delegate bool SamActionWaitForDelegate();
	public class SamAction {

		public float Length{get; set;} //the full length of this action. if it's 0, that means it'll run the next action instantly
		public float Timer{get; set;} // if timer is -1, that means it hasn't started yet
		public bool IsStarted{get; private set;}
		public bool IsFinished{get{return WaitFor == null ? Timer <= 0 : WaitFor();}}
		public string identifier = "";

		public SamActionDelegate OnStart{get; set;}
		public SamActionDelegate OnFinish{get; set;}
		public SamActionWaitForDelegate WaitFor{get; set;}
		public SamActionDelegate OnInterrupt{get; set;}

		public int Priority{get; set;}
		public float InterruptRadius{get; set;}
		public int RepeatTimes{get; set;}
		private int repeater;

		public SamAction(){
			Length = 0f;
			Timer = -1f;
			RepeatTimes = 1;
			repeater = 0;
		}
		public SamAction(SamActionDelegate onStart, SamActionDelegate onFinish = null) : this(0f, onStart, onFinish){}
		public SamAction(float length, SamActionDelegate onStart = null, SamActionDelegate onFinish = null) : this(){
			this.Length = length;
			this.OnStart = onStart;
			this.OnFinish = onFinish;
		}


		//not unity start, but it runs when vignette manager gets to this script
		public void Start(){
			repeater = 0;	//set repeater back down
			InternalStart();//begin the steps
			IsStarted = true;
		}

		private void InternalStart(){
			repeater++;
			Timer = Length;	//start the timer
			Act();	//do the actual action
		}

		public void Act(){
			if (OnStart != null) OnStart();
			if (identifier != "") Debug.Log(identifier);
		}

		//not unity update, but it does run on each unity update
		public virtual void Update(){
			UpdateTimer();
		}

		protected void UpdateTimer(){
			this.Timer -= Time.deltaTime;
		}

		public void Finish(){
			if (OnFinish != null) {
				OnFinish();
			}
		}

		public SamAction Repeat(int repeatTimes){
			this.RepeatTimes = repeatTimes;
			return this;
		}

		public void Interrupt(){
			if (OnInterrupt != null) OnInterrupt();
		}
		public SamAction SetWaitFor(SamActionWaitForDelegate waitFor){
			this.WaitFor = waitFor;
			return this;
		}
	}



	//just an action that waits
	public class WaitAction : SamAction{
		public WaitAction(float time, SamActionDelegate onStart = null, SamActionDelegate onFinish = null) : base(){
			Length = time;
			this.OnStart = onStart;
			this.OnFinish = onFinish;
		}
	}

	//Action to wait for X frames to pass
	public class WaitFrameAction : SamAction{
		public WaitFrameAction(int x, SamActionDelegate onStart = null, SamActionDelegate onFinish = null) : base(){
			Length = 1 + x;
			this.OnStart = onStart;
			this.OnFinish = onFinish;
		}

		protected void UpdateTimer(){
			this.Timer -= 1;
		}
	}


	//an action that just prints to console a string
	public class DebugAction : SamAction{
		public DebugAction(string message) : base(){
			OnStart = ()=>Debug.Log(message);
		}
	}

	//wait until the waitFor function returns true.
	public class WaitForAction : SamAction{
		public WaitForAction(SamActionWaitForDelegate waitFor, SamActionDelegate onStart = null, SamActionDelegate onFinish = null) : base(){
			this.WaitFor = waitFor;
			this.OnStart = onStart;
			this.OnFinish = onFinish;
		}
	}

	public class InsertedVignetteAction : WaitForAction{
		public Vignette vignette;
		public InsertedVignetteAction(Vignette v) : base(()=>{return v.IsFinished;}){
			this.vignette = v;
			this.OnStart = ()=>{if (!v.IsPlaying) {v.Play();}};
			this.OnInterrupt = ()=>{vignette.Stop();};
		}
	}

	public class BranchVignette : SamAction{
		Vignette ifTrue;
		Vignette ifFalse;
		public Vignette v;
		public BranchVignette(System.Func<bool> check, Vignette ifTrue, Vignette ifFalse){
			this.WaitFor = ()=>{return v.IsFinished;};
			this.ifTrue = ifTrue;
			this.ifFalse = ifFalse;
			this.OnStart = ()=>{
				v = check() ? ifTrue : ifFalse;
				if (!v.IsPlaying) {v.Play();}
			};
			this.OnInterrupt = ()=>{v.Stop();};
		}
		public BranchVignette(System.Func<bool> check, Vignette ifTrue){
			this.WaitFor = ()=>{return v.IsFinished;};
			this.ifTrue = ifTrue;
			this.v = ifTrue;
			this.OnStart = ()=>{
				if (!check()) {this.WaitFor = null; return;}
				else{
					if (!v.IsPlaying) {v.Play();}
				}
			};
			this.OnInterrupt = ()=>{v.Stop();};
		}
	}

	public class TimeOutWaitFor : SamAction{
		public TimeOutWaitFor(float timeout, SamActionWaitForDelegate waitfor) : base(){
			this.Length = timeout;
			this.WaitFor = ()=>{return(this.Timer <= 0 || waitfor());};
		}
	}


}