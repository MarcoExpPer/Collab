using UnityEditor;
using UnityEngine;

public static class DebugItemUpdater
{
    [MenuItem("Tools/Update Debug item database")]
    public static void GenerateDatabase()
    {
        // Create a new database
        DebugObtainableItemDatabase itemDatabase = ScriptableObject.CreateInstance<DebugObtainableItemDatabase>();

        // Find all ObtainableItem objects in the project
        string[] guids = AssetDatabase.FindAssets("t:ObtainableItem");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ObtainableItem item = AssetDatabase.LoadAssetAtPath<ObtainableItem>(path);
            itemDatabase.obtainableItems.Add(item);
        }

        // Save the database as an asset
        string assetPath = "Assets/Resources/Debug/ObtainableItemDatabase.asset";
        AssetDatabase.CreateAsset(itemDatabase, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Generated ObtainableItemDatabase with {itemDatabase.obtainableItems.Count} items.");
    }
}