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
        GUILayout.Label("MDMulti Information", EditorStyles.boldLabel);

        PropertyLabel("Services Loaded", MainMonoLoaded().ToString());

        GUILayout.Space(10);
        
        PropertyLabel("Rest Base URL", MDMulti.Rest.ServerUrl);
        PropertyLabel("Protocol Version", MDMulti.Rest.ProtocolVersion.ToString());

        GUILayout.Space(10);

        PropertyLabel("Multicasting?", MDMulti.EditorExternalFactors.MulticastActive.ToString());
        PropertyLabel("Broadcasting?", MDMulti.EditorExternalFactors.BroadcastActive.ToString());
    }

    void Update()
    {
        Repaint();
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
        return MDMulti.Mono.Main.Inst == null;
    }
}
