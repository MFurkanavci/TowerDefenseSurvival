using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using System.IO;

/// <summary>
/// CustomFolderCreator is a Unity Editor window that allows users to create, manage, export, and import custom folder structures.
/// </summary>
public class CustomFolderCreator : EditorWindow
{
    private int tab = 0;
    private Vector2 scrollPos;

    private string newStructureName = "New Structure";
    private List<string> subFolderNames = new List<string>();
    private int subFolderCount = 1;
    private static Dictionary<string, List<string>> folderStructures = new Dictionary<string, List<string>>();

    private string selectedRootFolder = "Assets";
    private string newFolderName = "New Folder";
    private string selectedStructureName = "";

    private string bulkNames = "";
    private string searchQuery = "";

    /// <summary>
    /// Displays the Custom Folder Creator window in the Unity Editor.
    /// </summary>
    [MenuItem("Tools/Custom Folder Creator")]
    public static void ShowWindow()
    {
        GetWindow<CustomFolderCreator>("Folder Creator");
        LoadFolderStructures();
    }

    /// <summary>
    /// Called by Unity to draw the GUI.
    /// </summary>
    private void OnGUI()
    {
        tab = GUILayout.Toolbar(tab, new string[] { "Create Structure", "Add Structure", "Bulk Create", "Manage Structures" });

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        switch (tab)
        {
            case 0:
                DrawCreateStructureTab();
                break;
            case 1:
                DrawAddStructureTab();
                break;
            case 2:
                DrawBulkCreateTab();
                break;
            case 3:
                DrawManageStructuresTab();
                break;
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// Draws the "Create Structure" tab.
    /// </summary>
    private void DrawCreateStructureTab()
    {
        GUILayout.Label("Create New Folder Structure", EditorStyles.boldLabel);
        newStructureName = EditorGUILayout.TextField("Structure Name", newStructureName);

        GUILayout.Label("Sub-Folders", EditorStyles.boldLabel);
        subFolderCount = EditorGUILayout.IntField("Number of Sub-Folders", subFolderCount);

        AdjustSubFolderNamesList();

        for (int i = 0; i < subFolderNames.Count; i++)
        {
            subFolderNames[i] = EditorGUILayout.TextField("Sub-Folder " + (i + 1), subFolderNames[i]);
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Save Structure"))
        {
            SaveFolderStructure();
        }

        GUILayout.Space(20);
        GUILayout.Label("Saved Structures", EditorStyles.boldLabel);

        List<string> keysToRemove = new List<string>();
        foreach (var structure in folderStructures.Where(s => s.Key.Contains(searchQuery)))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(structure.Key, EditorStyles.label);
            if (GUILayout.Button("Remove"))
            {
                keysToRemove.Add(structure.Key);
            }
            GUILayout.EndHorizontal();
        }

        foreach (var key in keysToRemove)
        {
            RemoveFolderStructure(key);
        }

        searchQuery = EditorGUILayout.TextField("Search Structures", searchQuery);
    }

    /// <summary>
    /// Draws the "Add Structure" tab.
    /// </summary>
    private void DrawAddStructureTab()
    {
        GUILayout.Label("Select Root Folder", EditorStyles.boldLabel);
        if (GUILayout.Button("Select Root Folder"))
        {
            string folderPath = EditorUtility.OpenFolderPanel("Select Root Folder", "Assets", "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                selectedRootFolder = "Assets" + folderPath.Substring(Application.dataPath.Length);
            }
        }
        GUILayout.Label("Root Folder: " + selectedRootFolder, EditorStyles.label);

        GUILayout.Label("Select Folder Structure", EditorStyles.boldLabel);
        foreach (var structure in folderStructures)
        {
            if (GUILayout.Button(structure.Key))
            {
                selectedStructureName = structure.Key;
            }
        }
        GUILayout.Label("Selected Structure: " + selectedStructureName, EditorStyles.label);

        newFolderName = EditorGUILayout.TextField("Main Folder Name", newFolderName);

        GUILayout.Space(20);

        if (GUILayout.Button("Create Folder Structure") && !string.IsNullOrEmpty(selectedStructureName))
        {
            CreateFolderStructure(selectedStructureName);
        }
    }

    /// <summary>
    /// Draws the "Bulk Create" tab.
    /// </summary>
    private void DrawBulkCreateTab()
    {
        GUILayout.Label("Select Root Folder", EditorStyles.boldLabel);
        if (GUILayout.Button("Select Root Folder"))
        {
            string folderPath = EditorUtility.OpenFolderPanel("Select Root Folder", "Assets", "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                selectedRootFolder = "Assets" + folderPath.Substring(Application.dataPath.Length);
            }
        }
        GUILayout.Label("Root Folder: " + selectedRootFolder, EditorStyles.label);

        GUILayout.Label("Enter Folder Names", EditorStyles.boldLabel);
        bulkNames = EditorGUILayout.TextArea(bulkNames);

        GUILayout.Label("Select Folder Structure", EditorStyles.boldLabel);
        foreach (var structure in folderStructures)
        {
            if (GUILayout.Button(structure.Key))
            {
                selectedStructureName = structure.Key;
            }
        }
        GUILayout.Label("Selected Structure: " + selectedStructureName, EditorStyles.label);

        GUILayout.Space(20);

        if (GUILayout.Button("Create Folder Structure") && !string.IsNullOrEmpty(selectedStructureName))
        {
            var names = bulkNames.Split('\n').Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n));
            foreach (var name in names)
            {
                CreateFolderStructureForBulk(name);
            }
        }
    }

    /// <summary>
    /// Draws the "Manage Structures" tab.
    /// </summary>
    private void DrawManageStructuresTab()
    {
        GUILayout.Label("Manage Saved Structures", EditorStyles.boldLabel);

        if (GUILayout.Button("Export Structures"))
        {
            ExportStructures();
        }

        if (GUILayout.Button("Import Structures"))
        {
            ImportStructures();
        }
    }

    /// <summary>
    /// Adjusts the size of the subFolderNames list to match the subFolderCount.
    /// </summary>
    private void AdjustSubFolderNamesList()
    {
        while (subFolderNames.Count < subFolderCount)
        {
            subFolderNames.Add("Sub-Folder " + (subFolderNames.Count + 1));
        }
        while (subFolderNames.Count > subFolderCount)
        {
            subFolderNames.RemoveAt(subFolderNames.Count - 1);
        }
    }

    /// <summary>
    /// Saves the current folder structure.
    /// </summary>
    private void SaveFolderStructure()
    {
        if (string.IsNullOrEmpty(newStructureName))
        {
            Debug.LogError("Structure name cannot be empty.");
            return;
        }

        folderStructures[newStructureName] = new List<string>(subFolderNames);
        SaveFolderStructuresToPrefs();
        Debug.Log("Structure saved successfully!");
    }

    /// <summary>
    /// Removes a folder structure by name.
    /// </summary>
    /// <param name="structureName">The name of the structure to remove.</param>
    private void RemoveFolderStructure(string structureName)
    {
        if (folderStructures.ContainsKey(structureName))
        {
            folderStructures.Remove(structureName);
            SaveFolderStructuresToPrefs();
            Debug.Log("Structure removed successfully!");
        }
    }

    /// <summary>
    /// Saves all folder structures to EditorPrefs.
    /// </summary>
    private static void SaveFolderStructuresToPrefs()
    {
        EditorPrefs.DeleteKey("FolderStructures");

        foreach (var structure in folderStructures)
        {
            string key = "FolderStructure_" + structure.Key;
            string value = string.Join(";", structure.Value);
            EditorPrefs.SetString(key, value);
        }
        EditorPrefs.SetString("FolderStructureNames", string.Join(";", folderStructures.Keys));
    }

    /// <summary>
    /// Loads folder structures from EditorPrefs.
    /// </summary>
    private static void LoadFolderStructures()
    {
        folderStructures.Clear();
        string structureNames = EditorPrefs.GetString("FolderStructureNames", "");
        if (!string.IsNullOrEmpty(structureNames))
        {
            var names = structureNames.Split(';');
            foreach (var name in names)
            {
                string key = "FolderStructure_" + name;
                string value = EditorPrefs.GetString(key, "");
                if (!string.IsNullOrEmpty(value))
                {
                    folderStructures[name] = new List<string>(value.Split(';'));
                }
            }
        }
    }

    /// <summary>
    /// Creates a folder structure based on the selected structure name.
    /// </summary>
    /// <param name="structureName">The name of the structure to create.</param>
    private void CreateFolderStructure(string structureName)
    {
        if (!AssetDatabase.IsValidFolder(selectedRootFolder))
        {
            Debug.LogError("Selected root folder is invalid.");
            return;
        }

        string rootPath = selectedRootFolder + "/" + newFolderName;

        if (!AssetDatabase.IsValidFolder(rootPath))
        {
            AssetDatabase.CreateFolder(selectedRootFolder, newFolderName);
        }

        foreach (string subFolderName in folderStructures[structureName])
        {
            string subFolderPath = rootPath + "/" + subFolderName;

            if (!AssetDatabase.IsValidFolder(subFolderPath))
            {
                AssetDatabase.CreateFolder(rootPath, subFolderName);
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("Folder structure created successfully at " + rootPath);
    }

    /// <summary>
    /// Creates a folder structure for bulk creation based on the provided folder name.
    /// </summary>
    /// <param name="folderName">The name of the main folder to create.</param>
    private void CreateFolderStructureForBulk(string folderName)
    {
        if (!AssetDatabase.IsValidFolder(selectedRootFolder))
        {
            Debug.LogError("Selected root folder is invalid.");
            return;
        }

        string rootPath = selectedRootFolder + "/" + folderName;

        if (!AssetDatabase.IsValidFolder(rootPath))
        {
            AssetDatabase.CreateFolder(selectedRootFolder, folderName);
        }

        foreach (string subFolderName in folderStructures[selectedStructureName])
        {
            string subFolderPath = rootPath + "/" + subFolderName;

            if (!AssetDatabase.IsValidFolder(subFolderPath))
            {
                AssetDatabase.CreateFolder(rootPath, subFolderName);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Folder structure created successfully at " + rootPath);
    }

    /// <summary>
    /// Exports the saved folder structures to a JSON file.
    /// </summary>
    private void ExportStructures()
    {
        string filePath = EditorUtility.SaveFilePanel("Export Structures", "", "folderStructures", "json");
        if (!string.IsNullOrEmpty(filePath))
        {
            var data = new FolderStructureData(folderStructures);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Structures exported successfully to " + filePath);
        }
    }

    /// <summary>
    /// Imports folder structures from a JSON file.
    /// </summary>
    private void ImportStructures()
    {
        string filePath = EditorUtility.OpenFilePanel("Import Structures", "", "json");
        if (!string.IsNullOrEmpty(filePath))
        {
            string json = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<FolderStructureData>(json);
            folderStructures = data.Structures.ToDictionary(s => s.Name, s => s.SubFolders);
            SaveFolderStructuresToPrefs();
            Debug.Log("Structures imported successfully from " + filePath);
        }
    }

    /// <summary>
    /// Data class to represent the folder structures for serialization.
    /// </summary>
    [System.Serializable]
    public class FolderStructureData
    {
        public List<FolderStructure> Structures = new List<FolderStructure>();

        /// <summary>
        /// Constructor that initializes the data from a dictionary of folder structures.
        /// </summary>
        /// <param name="structures">The folder structures to initialize with.</param>
        public FolderStructureData(Dictionary<string, List<string>> structures)
        {
            foreach (var kvp in structures)
            {
                Structures.Add(new FolderStructure { Name = kvp.Key, SubFolders = kvp.Value });
            }
        }
    }

    /// <summary>
    /// Data class to represent a single folder structure.
    /// </summary>
    [System.Serializable]
    public class FolderStructure
    {
        public string Name;
        public List<string> SubFolders;
    }
}

