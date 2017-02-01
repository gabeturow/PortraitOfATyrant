//=============================================================================
// ListSeriablizer
// Copyright Pigasus Games
// Author Petrucio
//
// Basic Usage:
//  (Setup)
// 1. Add this script to a game object with a list that you want to edit in text outside Unity
// 2. Set the type of values your list contains in the "List Type" dropdown (string by default)
// 3. Add these export and import methods to the script containing the list:
//	    void ListImport(string[] values) { this.list = values; }
//	    void ListExport(Pigasus.ListSerializer serializer) { serializer.Export<string>(this.list); }
// 4. Add [ExecuteInEditMode] before your target class if you want to be able to change the list during edit mode.
//  (Usage)
// 1. Click on "Export Values Now" to save the values to a ListSerializer.xml file (on the project root by default)
// 2. Editing the list values in the XML file
// 3. Click on "Import Values Now" to load the values into the list
//
// You can change the default names of the export methods and XML files in the options fold.
// See the ListSerializer.pdf for more info.
// 
// Possible error messages you'll see when trying to import/export, and their meanings:
//   ShouldRunBehaviour () - you are trying to export or import values for a script that does not have the [ExecuteInEditMode] marker
//   SendMessage XYZ has no receiver! - you did not add the import or export methods to the target script with the list (see above)
//   XmlException: XYZ - You messed up with the XML syntax when editing the XML file
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

#if UNITY_EDITOR

namespace Pigasus {

//=============================================================================
[ExecuteInEditMode]
public class ListSerializer : MonoBehaviour {
		
	public enum ListType {
		String,
		Integer,
		Float,
		Vector3,
			
		// Add custom types here:
		SampleType,
	}
	public ListType listType = ListType.String;
		
	[System.Serializable]
	public class Options {
		public GameObject targetObj;
		public string     targetMsgImport = "ListImport";
		public string     targetMsgExport = "ListExport";
		public string     targetFile = null;
	}
	public Options options = new Options();
		
	// TODO: Make a nicer editor UI with buttons for these
	public bool _exportValuesNow = false;
	public bool _importValuesNow = false;
	
	//=============================================================================
	void Start()
	{
		if (string.IsNullOrEmpty(this.options.targetFile)) {
			this.options.targetFile = "./" + this.name + "_ListSerializer.xml";
		}
	}

	//=============================================================================
	void Update()
	{
		if (this._importValuesNow) {
			this._importValuesNow = false;
			this.Import();
		}
		if (this._exportValuesNow) {
			this._exportValuesNow = false;
			this.ExportStart();
		}
	}
	
	//=============================================================================
	// TODO: this would probably be easier to do in UnityScript, to use weak typing and remove the need for listType? Note to self: UnityScript sucks
	public void Import()
	{
		switch (this.listType)
		{
		case ListType.Float:   this.Import<float>();   break;
		case ListType.Integer: this.Import<int>();     break;
		case ListType.String:  this.Import<string>();  break;
		case ListType.Vector3: this.Import<Vector3>(); break;
				
		// Add custom types here:
		case ListType.SampleType: this.Import<SampleUsage.MyType>(); break;
				
		default: Debug.LogError("Unkown Import type"); break;
		}
	}
	
	//=============================================================================
	private void Import<T>()
	{
		string saveState = System.IO.File.ReadAllText(this.options.targetFile);
		T[] values = XMLExtensions.DeserializeObject<T[]>(saveState);
		
		Debug.Log("ListSerializer loaded values from: " + this.options.targetFile);

		if (!this.options.targetObj) this.options.targetObj = this.gameObject;
		this.options.targetObj.SendMessage(this.options.targetMsgImport, values, SendMessageOptions.RequireReceiver);
	}
	
	//=============================================================================
	public void ExportStart()
	{
		if (!this.options.targetObj) this.options.targetObj = this.gameObject;
		this.options.targetObj.SendMessage(this.options.targetMsgExport, this);
	}
		
	//=============================================================================
	public void Export<T>(T[] values)
	{
		string saveState = values.SerializeObject();
		System.IO.File.WriteAllText(this.options.targetFile, saveState);
			
		Debug.Log("ListSerializer saved values to: " + this.options.targetFile);
	}
	
// Just for quick testing and debugging
#if false
	public string[] testStrList = new string[] { "string 1", "string 2" };
	
	//=============================================================================
	public void ListImport(string[] values)
	{
		this.testStrList = values;
	}
		
	//=============================================================================
	public void ListExport(Pigasus.ListSerializer serializer)
	{
		serializer.Export<string>(this.testStrList);
	}
#endif
	
}

#endif

//=========================================================================
public static class XMLExtensions
{
	//=========================================================================
	// http://stackoverflow.com/questions/2434534/serialize-an-object-to-string
	public static string SerializeObject<T>(this T toSerialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
		StringWriter textWriter = new StringWriter();
		
		xmlSerializer.Serialize(textWriter, toSerialize);
		return textWriter.ToString();
	}
	
	public static T DeserializeObject<T>(string serializedString)
	{
		XmlSerializer xmlDeserializer = new XmlSerializer(typeof(T));
		StringReader textReader = new StringReader(serializedString);
		
		return (T) xmlDeserializer.Deserialize(textReader);
	}
	
}

} // namespace

