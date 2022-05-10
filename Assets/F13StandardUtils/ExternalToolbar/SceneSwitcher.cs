#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace F13StandardUtils.ExternalToolbar
{
    public static class ToolbarStyles
    {
        public static GUIStyle SmallCommandButtonStyle() =>new GUIStyle("Command")
        {
            fixedWidth = 20,
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle MediumCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 40,
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle LargeCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 60,
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle XLargeCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 80,
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
    }

    [InitializeOnLoad]
    public class ToolbarSwitchButton : IPreprocessBuildWithReport
    {

        static ToolbarSwitchButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUILeft);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIRight);
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        }

        static List<SceneAsset> scenes = new List<SceneAsset>();

        static void OnToolbarGUILeft()
        {

            GUILayout.BeginHorizontal();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (GUILayout.Button(new GUIContent(i.ToString()), ToolbarStyles.SmallCommandButtonStyle()))
                {
                    SceneHelper.OpenScene(EditorBuildSettings.scenes[i].path);
                }
            }
            GUILayout.EndHorizontal();
        }

        static void OnToolbarGUIRight()
        {

            GUILayout.BeginHorizontal();
            
            AddButtonOnToolbar("SaveAssets", Color.magenta, ToolbarStyles.XLargeCommandButtonStyle(), 
                (buttonName,buttonColor,buttonGuiStyle) =>
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });
            
            AddButtonOnToolbar("OnEveryTime", Color.blue, ToolbarStyles.XLargeCommandButtonStyle(), 
                (buttonName,buttonColor,buttonGuiStyle) =>
            {
                //DO SOME STUFF
                Debug.Log("Şu anda her zaman bu button görünür");
            });
            
            AddEditModeButtonOnToolbar("OnEditMode",Color.red, ToolbarStyles.XLargeCommandButtonStyle(), 
                (buttonName,buttonColor,buttonGuiStyle) =>
                {
                    //DO SOME STUFF
                    Debug.Log("Şu anda edit moddayken bu button görünür");
                });
            
            
            AddPlayModeButtonOnToolbar("OnPlayMode",Color.green ,ToolbarStyles.XLargeCommandButtonStyle(), 
                (buttonName,buttonColor,buttonGuiStyle) =>
            {
                //DO SOME STUFF
                Debug.Log("Şu anda play mode açıkken bu button görünür");
            });
            


            GUILayout.EndHorizontal();
        }

        private static void AddPlayModeButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            if (Application.isPlaying)
            {
                AddButtonOnToolbar(buttonName,color,guiStyle,onClick);
            }
        }
        
        private static void AddEditModeButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            if (!Application.isPlaying)
            {
                AddButtonOnToolbar(buttonName,color,guiStyle,onClick);
            }
        }

        private static void AddButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            guiStyle.normal.textColor = color;
            var guiContent = new GUIContent(buttonName);
            if (GUILayout.Button(guiContent, guiStyle))
            {
                onClick?.Invoke(buttonName,color,guiStyle);
            }
        }

        public int callbackOrder { get; }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    //DO Some stuff when enter editmode !
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    //DO Some stuff when exit editmode before press play !
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    //DO Some stuff when enter play mode after press play !
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    //DO Some stuff when exit play mode after press stop!
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }
        public void OnPreprocessBuild(BuildReport report)
        {
            //DO Some stuff before BUILDING!!!!!
        }
    }
    
    static class SceneHelper
    {
        static string sceneToOpen;

        public static void OpenScene(string scene)
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scene);
        }
    }
}
#endif
