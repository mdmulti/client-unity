using UnityEngine;
using UnityEditor;

namespace MDMulti.Editor
{
    public class MDMultiInfo : Core
    {
        bool fp1 = true;
        bool fp2 = true;

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

            fp1 = EditorGUILayout.Foldout(fp1, "Multicast");
            if (fp1)
            {
                PropertyLabel("     Sending?", Factors.ActiveItems.MulticastSend);
                PropertyLabel("     Receiving?", Factors.ActiveItems.MulticastRecv);
            }

            fp2 = EditorGUILayout.Foldout(fp2, "Broadcast");
            if (fp2)
            {
                PropertyLabel("     Sending?", Factors.ActiveItems.BroadcastSend);
                PropertyLabel("     Receiving?", Factors.ActiveItems.BroadcastRecv);
            }

        }

        void Update()
        {
            Repaint();
        }

        public void OnInspectorUpdate()
        {
            Update();
        }
    }
}