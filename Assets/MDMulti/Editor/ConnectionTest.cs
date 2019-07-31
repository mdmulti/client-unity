using UnityEngine;
using UnityEditor;
using MDMulti;
using MDMulti.Editor;

public class ConnectionTest : Core
{
    private static readonly string helpMessage = "MDMulti Services must be running to perform a connection test!";

    [MenuItem("MDMulti/Connection Test")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ConnectionTest window = (ConnectionTest)GetWindow(typeof(ConnectionTest));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Connection Test", EditorStyles.boldLabel);

        PropertyLabel("URL", Rest.ServerUrl);
        
        if (GUILayout.Button("Test"))
        {
            try
            {
                MDMulti.Mono.Main.Inst.StartCoroutine(Rest.Get("info", res =>
                {
                    EditorUtility.DisplayDialog("MDMulti Connection Test", "Connection test " + ((res.ResponseCode() == 200) ? "passed!" : "failed."), "OK", "");
                }));
            } catch (System.Exception)
            {
                EditorUtility.DisplayDialog("MDMulti Connection Test", helpMessage, "OK");
            }
        }
        EditorGUILayout.HelpBox(helpMessage, MessageType.Warning);
    }

    void Update()
    {
        Repaint();
    }
}