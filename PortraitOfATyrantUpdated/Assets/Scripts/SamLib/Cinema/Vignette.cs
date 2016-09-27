using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SamCinema{
	//a vignette is a chain of actions 
	//it's all in a linked list, plays through sequentially
	//add one to SamCinemaManager to start playing it
	public class Vignette{
		private LinkedList<SamAction> actionList;

		public SamAction CurrentAction{get{return actionList.Count != 0 ? actionList.First.Value : null;}}
		public bool IsFinished{get{return actionList.Count == 0;}}
		public bool IsPlaying{get{return SamCinemaManager.main.IsRunning(this);}}
		public int RepeatTimes{get; set;}
		private int repeater;

		public Vignette(){
			actionList = new LinkedList<SamAction>();
			Add(new WaitAction(0f));
			RepeatTimes = 1;
		}

		public Vignette(params SamAction[] actions) : this(){
			for(int i = 0; i < actions.Length; i++){
				actionList.AddLast(actions[i]);
			}
		}

		public void Clear(){
			actionList = new LinkedList<SamAction>();
		}

		protected Vignette(LinkedList<SamAction> actionList) : this(){
			for(LinkedListNode<SamAction> node = actionList.First; node != null; node = node.Next){
				Add(node.Value);
			}
		}

		public Vignette Copy(){
			return new Vignette(actionList);
		}

		public Vignette Play(){
			if (!IsPlaying)
				SamCinemaManager.main.AddVignette(this);
			return this;
		}

		public Vignette Stop(){
			if (CurrentAction != null) {CurrentAction.Interrupt();
				if (actionList.First.Next != null) actionList.First.Next.Value.Interrupt();
			}
			SamCinemaManager.main.RemoveVignette(this);
			return this;
		}

		//called by CinemaManager.
		//Doesn't actually play the vignette
		public void InternalPlay(){
			repeater = 0;
			InternalStart();
		}

		private void InternalStart(){
		}

		//don't call this yourself.
		//called by samcinemamanager's update.
		public void Update(){
//			if (!startedCurrentAction){
//				RestartCurrentAction();
//			}
			if (CurrentAction != null){
				if (!CurrentAction.IsStarted) CurrentAction.Start();
				CurrentAction.Update();
				if (CurrentAction.IsFinished){
					NextAction();
					Update();
				}
			}
		}

		public SamAction Add(string identifier, SamAction action){
			SamAction a = Add(action);
			a.identifier = identifier;
			return a;
		}
		public SamAction Add(string identifier, System.Action action){
			SamAction a = Add(action);
			a.identifier = identifier;
			return a;
		}
		//add an action. to the back of the list.
		public SamAction Add(SamAction action){
			return actionList.AddLast(action).Value;
		}
		public SamAction Add(Vignette v){
			return Add (new InsertedVignetteAction(v));
		}
		public SamAction Add(System.Action action){
			return actionList.AddLast(new SamAction(0f, ()=>{action();})).Value;
		}

		//remove an action.
		//will search the list.
		public void RemoveAction(SamAction action){
			actionList.Remove(action);
		}


		//insert the action NOW.
		// will interrupt current action. current action will be played after this action inserted.
		public Vignette InsertActionFirst(SamAction action, bool interrupt = true){
			if (interrupt && CurrentAction != null) CurrentAction.Interrupt();
			actionList.AddFirst(action);
			return this;
		}

		public Vignette InsertActionLast(SamAction action){
			actionList.AddLast(action);
			return this;
		}

		/// <summary>
		/// Inserts the vignette into this vignette.
		/// If interrupt = true, it inserts it and replaces the current action, shifting the current action to after.
		/// If false it will continue the current action until it is done.
		/// </summary>
		public Vignette InsertVignetteFirst(Vignette v, bool interrupt = true){


//			//check if you're inserting inside of an inserted vignette.
//			if (CurrentAction.GetType() == typeof(InsertedVignetteAction)){
//				InsertedVignetteAction runningVignette = CurrentAction as InsertedVignetteAction;
//				runningVignette.vignette.InsertVignetteFirst(v, interrupt);
//				return this;
//			}

			if (interrupt) CurrentAction.Interrupt();

			//otherwise just insert into this vignette.
			InsertActionFirst(new InsertedVignetteAction(v));
			if (IsPlaying) {StartCurrentAction();}
			return this;
		}

		public Vignette InsertVignetteLast(Vignette v){
			InsertActionLast(new InsertedVignetteAction(v));
			return this;
		}

		//restart the current action
		public void RestartCurrentAction(){
			StartCurrentAction();
		}

		void StartCurrentAction(){
			if (CurrentAction != null){
				CurrentAction.Start();
			}
		}


		//advance the action to the next one.
		public void NextAction(){
			SamAction finishedAction = CurrentAction;
			actionList.RemoveFirst();
			finishedAction.Finish();
			StartCurrentAction();
		}


		public Vignette Repeat(int times){
			this.RepeatTimes = times;
			return this;
		}


	}


}