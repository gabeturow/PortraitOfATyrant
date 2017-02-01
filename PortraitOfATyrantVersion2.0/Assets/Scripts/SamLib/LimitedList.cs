using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LimitedList<T> : List<T> {
	public int MaxCapacity{get; private set;}
	public LimitedList(int maxCapacity){
		MaxCapacity = maxCapacity;
	}

	new public bool Add(T item){
		if (this.Count < this.MaxCapacity){
			base.Add(item);
			return true;
		}
		return false;
	}

}
