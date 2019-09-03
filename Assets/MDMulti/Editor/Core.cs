using UnityEditor;
using UnityEngine;

namespace MDMulti.Editor
{
    public class Core : EditorWindow
    {
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
        public static void PropertyLabel(string name, string value)
        {
            const int labelHeight = 18;
            const string missingValue = "<empty>";

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(name);
            EditorGUILayout.SelectableLabel(string.IsNullOrEmpty(value) ? missingValue : value, GUILayout.Height(labelHeight));
            EditorGUILayout.EndHorizontal();
        }

        public static void PropertyLabel(string name, bool value)
        {
            PropertyLabel(name, value.ToString());
        }

        public static bool MainMonoLoaded()
        {
            return Mono.Main.Inst != null;
        }
    }
}