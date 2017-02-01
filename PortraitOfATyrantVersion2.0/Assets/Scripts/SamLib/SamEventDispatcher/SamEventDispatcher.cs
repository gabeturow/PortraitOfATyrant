using System;
using System.Collections.Generic;

//update this guy in the main thread
public static class SamEventDispatcher{

	private static Queue<Action> q = new Queue<Action>();

	public static void SlowUpdate(){
		if (q.Count > 0){
			var e = q.Dequeue();
			e();
		}
	}

	public static void FastUpdate(){
		while(q.Count > 0){
			var e = q.Dequeue();
			e();
		}
	}

	public static void RunOnMain(System.Action action){
		q.Enqueue(action);
	}

	public static void RaiseOnMain<T>(this System.EventHandler<T> handler, object sender, T args) where T : System.EventArgs {
		q.Enqueue(()=>{if (handler != null) handler(sender, args);});
 	 }
//	public static void RaiseOnMain(this System.EventHandler handler, object sender, System.EventArgs args) {
//		q.Enqueue(()=>{if (handler != null) handler(sender, args);});
// 	 }
	public static void RaiseOnMain(this System.EventHandler handler, object sender){
		q.Enqueue(()=>{if (handler != null) handler(sender, System.EventArgs.Empty);});
	}


}
