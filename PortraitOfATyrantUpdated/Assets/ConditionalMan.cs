using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConditionalMan {


	Dictionary<string, Conditional> conditionals {get; set;}

	public ConditionalMan(){
		conditionals = new Dictionary<string, Conditional>();
	}


	public void SetValue(string key, bool value){
		GetValue(key).Set(value.ToString());
	}

	public void SetValue(string key, string value){
		GetValue(key).Set(value);
	}

	/// <summary>
	/// Gets the value for a conditional.
	/// </summary>
	/// <returns><c>true</c>, if value was gotten, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	public Conditional GetValue(string key){
		Conditional ans = null;
		if (conditionals.ContainsKey(key)){
			ans = conditionals[key];
		}
		else{
			var newConditional = new Conditional(key);
			conditionals.Add(key, newConditional);
			ans = newConditional;
		}
		return ans;
	}

	public SaveFile.Conditional[] ToList(){
		var ans = new List<SaveFile.Conditional>();
		var keys = conditionals.Keys;
		foreach (var key in keys) {
			ans.Add(GetValue(key));
		}
		return ans.ToArray();
	}

	public void FromList(SaveFile.Conditional[] list){
		for (int i = 0; i < list.Length; i++) {
			var element = list[i];
			Debug.Log(element);
			SetValue(element.key, element.value);
		}
	}

	[System.Serializable]
	public class Conditional {
		public string key 		{ get; private set; }
		public string value 	{ get; private set; }
		public Conditional(string key, string value = "false"){
			this.key = key;
			this.value = value;
		}
		public void Set(string value){
			this.value = value;
		}
		public void Set(bool value){
			this.value = value.ToString().ToLower();
		}
		public void Set(float value){
			this.value = value.ToString();
		}
		public void Set(int value){
			this.value = value.ToString();
		}
		public bool GetBool(){
			return value.ToLower() != "false";
		}
		public int GetInt(){
			return int.Parse(value);
		}
		public float GetFloat(){
			return float.Parse(value);
		}
		public string GetString(){
			return value;
		}
		public static implicit operator bool(Conditional conditional){
			return conditional.GetBool();
		}
		public static implicit operator int(Conditional conditional){
			return conditional.GetInt();
		}
		public static implicit operator float(Conditional conditional){
			return conditional.GetFloat();
		}
		public static implicit operator string(Conditional conditional){
			return conditional.value;
		}
		public static implicit operator SaveFile.Conditional(Conditional conditional){
			return new SaveFile.Conditional {
				key = conditional.key,
				value = conditional.value
			};
		}
		public static implicit operator Conditional(SaveFile.Conditional conditional){
			return new Conditional(conditional.key, conditional.value);
		}
	}
}

