using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConditionalMan {

	public Dictionary<string, bool> conditionals {get; private set;}

	public ConditionalMan(){
		conditionals = new Dictionary<string, bool>();
	}


	/// <summary>
	/// Sets a conditional.
	/// </summary>
	public void SetValue(string key, bool value){
		conditionals[key] = value;
	}

	/// <summary>
	/// Sets a conditional to be true.
	/// </summary>
	public void SetValue(string key){
		conditionals[key] = true;
	}

	/// <summary>
	/// Gets the value for a conditional.
	/// </summary>
	/// <returns><c>true</c>, if value was gotten, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	public bool GetValue(string key){
		bool ans = false;
		if (conditionals.ContainsKey(key)){
			conditionals.TryGetValue(key, out ans);
		}
		else{
			conditionals.Add(key, false);
		}
		return ans;
	}


}
