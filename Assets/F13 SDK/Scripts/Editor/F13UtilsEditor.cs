using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

public class F13UtilsEditor : UnityEditor.EditorWindow
{
    public bool isUIManager = true;
    public bool isObjectManager = false;

    [MenuItem("F13/Import Settings")]
   static void OpenWindow()
    {
        F13UtilsEditor utilsWindow = (F13UtilsEditor)GetWindow(typeof(F13UtilsEditor));
        utilsWindow.minSize = new Vector2(400, 400);
        utilsWindow.maxSize = new Vector2(400, 800);
    }

    private void OnEnable()
    {
        VisualElement root = rootVisualElement;

        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
   ("Resources/Main.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

    //    var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
    //("Resources/style.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        //labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);
    }

    private void OnGUI()
    {
        //DrawHeader();
        //DrawLayouts();

    }

    private void SetupButton(Button button)
    {
        // Reference to the VisualElement inside the button that serves
        // as the button’s icon.
        var buttonIcon = button.Q(className: "quicktool-button-icon");

        // Icon’s path in our project.
        var iconPath = "Icons/" + button.parent.name + "-icon";

        // Loads the actual asset from the above path.
        var iconAsset = Resources.Load<Texture2D>(iconPath);

        // Applies the above asset as a background image for the icon.
        buttonIcon.style.backgroundImage = iconAsset;

        // Instantiates our primitive object on a left click.
        button.clickable.clicked += () => CreateObject(button.parent.name);

        // Sets a basic tooltip to the button itself.
        button.tooltip = button.parent.name;
    }

    private void CreateObject(string primitiveTypeName)
    {
        var pt = (PrimitiveType)Enum.Parse
                     (typeof(PrimitiveType), primitiveTypeName, true);
        var go = ObjectFactory.CreatePrimitive(pt);
        go.transform.position = Vector3.zero;
    }


    private void DrawLayouts()
    {
        isObjectManager = EditorGUILayout.Toggle("Object Manager", isObjectManager);
        isUIManager = EditorGUILayout.Toggle("UI Manager", isObjectManager);
        
    }


    private void DrawHeader()
    {
        GUILayout.Label("F13 SDK Entegration Settings", EditorStyles.boldLabel);
    }
}
