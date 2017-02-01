using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OtterListTest2 : MonoBehaviour
{
	[System.Serializable]
	public class SerializableInnerClass
	{
		public int publicInt = 0;
		[SerializeField] private int serializedInt = 1;
	}

	[SerializeField]
	public List<SerializableInnerClass> _list = new List<SerializableInnerClass>();
}
