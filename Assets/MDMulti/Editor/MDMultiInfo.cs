using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MDMultiInfo : EditorWindow {

    [MenuItem("MDMulti/Info")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MDMultiInfo window = (MDMultiInfo)GetWindow(typeof(MDMultiInfo));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Main Mono Information", EditorStyles.boldLabel);
        PropertyLabel("Loaded", MainMonoLoaded().ToString());

        GUILayout.Space(10);

        GUILayout.Label("Rest Information", EditorStyles.boldLabel);
        PropertyLabel("Base URL", MDMulti.Rest.ServerUrl);
        PropertyLabel("Protocol Version", MDMulti.Rest.ProtocolVersion.ToString());
    }

    /*
     * PropertyLabel function
     * 
     * Lifted from Microsoft's Xbox Live Unity Plugin
     * 
     * Licensed under MIT
     * Copyright (c) 2017 Microsoft Corporation
     * 
     * Assets/Xbox Live/Editor/XboxLiveConfigurationEditor.cs : 230 : d8626f5
    */
    private static void PropertyLabel(string name, string value)
    {
        const int labelHeight = 18;
        const string missingValue = "<empty>";

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(name);
        EditorGUILayout.SelectableLabel(string.IsNullOrEmpty(value) ? missingValue : value, GUILayout.Height(labelHeight));
        EditorGUILayout.EndHorizontal();
    }

    public static bool MainMonoLoaded()
    {
        if (MDMulti.MainMono.Mono == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
