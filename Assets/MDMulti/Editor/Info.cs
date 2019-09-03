using UnityEngine;
using UnityEditor;

namespace MDMulti.Editor
{
    public class MDMultiInfo : Core
    {

        [MenuItem("MDMulti/Info")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            MDMultiInfo window = (MDMultiInfo)GetWindow(typeof(MDMultiInfo), false, "MDMulti Info");
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("MDMulti Information", EditorStyles.boldLabel);
            
            if (Application.isPlaying)
            {
                PropertyLabel("Services Loaded", MainMonoLoaded().ToString());
            } else
            {
                PropertyLabel("Services Loaded", "False (Game not running!)");
            }
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
}