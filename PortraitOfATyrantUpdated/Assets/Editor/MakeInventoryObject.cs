using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeInventoryObject {
    [MenuItem("Assets/Create/New Inventory Item")]
    public static void CreateMyAsset()
    {
		InventoryObject asset = ScriptableObject.CreateInstance<InventoryObject>();

        AssetDatabase.CreateAsset(asset, "Assets/NewInventoryItem.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}