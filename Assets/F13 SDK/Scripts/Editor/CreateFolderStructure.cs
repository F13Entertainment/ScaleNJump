using UnityEditor;
using System.Collections.Generic;

public class CreateFolderStructure : ScriptableWizard
{

    public string FolderName = "Prototip Name";
    private string SFGUID;
    List<string> folders = new List<string>() { "Art", "Scripts", "3rdParty", "Scenes", "Prefabs" };
    List<string> ArtFolders = new List<string>() { "Animation", "Audio", "Sprites", "Materials", "Meshes", "Mesh Prefabs", "Texture", "Shaders", "Particles"};
    List<string> scriptsFolders = new List<string>() { "Player", "Core", "Enviroment", "Shaders" };
    List<string> animationFolders = new List<string>() { "Animators", "Clips" };
    List<string> thirdPartyFolders = new List<string>() { "Tools" };
    List<string> particleFolders = new List<string>() { "Materials", "Prefabs", "Textures", "Meshes" };

    [MenuItem("F13/Create Project Folders %#.")]
    static void CreateWizard()
    {
        DisplayWizard("Create Project Folders", typeof(CreateFolderStructure), "Create");
    }

    //Called when the window first appears
    void OnEnable()
    {

    }
    //Create button click
    void OnWizardCreate()
    {
        // creates the primary folder for game
        string primaryFolder = AssetDatabase.CreateFolder("Assets", FolderName);

        //create all the folders required in a project
        foreach (string folder in folders)
        {
            string guid = AssetDatabase.CreateFolder("Assets/" + FolderName, folder);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
            if (folder == "Scripts")
                SFGUID = newFolderPath;
        }

        AssetDatabase.Refresh();

        foreach (string art in ArtFolders)
        {
            //AssetDatabase.Contain
            string guid = AssetDatabase.CreateFolder("Assets/" + FolderName + "/Art", art);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

        }

        AssetDatabase.Refresh();

        foreach (string script in scriptsFolders)
        {
            //AssetDatabase.Contain
            string guid = AssetDatabase.CreateFolder("Assets/" + FolderName + "/Scripts", script);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

        }
        foreach (string animation in animationFolders)
        {
            //AssetDatabase.Contain
            string guid = AssetDatabase.CreateFolder("Assets/" + FolderName + "/Art/Animation", animation);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

        }

        foreach (string thirdParty in thirdPartyFolders)
        {
            //AssetDatabase.Contain
            string guid = AssetDatabase.CreateFolder("Assets/" + FolderName + "/3rdParty", thirdParty);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
        }

        foreach (string particles in particleFolders)
        {
            //AssetDatabase.Contain
            string guid = AssetDatabase.CreateFolder("Assets/" + FolderName + "/Art/Particles", particles);
            string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

        }

        AssetDatabase.Refresh();

    }
}
