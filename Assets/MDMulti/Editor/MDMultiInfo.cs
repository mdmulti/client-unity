using UnityEngine;
using UnityEditor;
using MDMulti.Editor;

public class MDMultiInfo : Core
{

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
}