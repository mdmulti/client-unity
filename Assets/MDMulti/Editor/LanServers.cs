using UnityEngine;
using UnityEditor;
using MDMulti.LAN.Discovery;

namespace MDMulti.Editor
{
    public class LANServers : Core
    {

        private bool isRegistered = false;

        [MenuItem("MDMulti/LAN Discovery")]
        static void Init()
        {
            //ServerFoundEvent sfe = new ServerFoundEvent();
            
            // Get existing open window or if none, make a new one:
            LANServers window = (LANServers)GetWindow(typeof(LANServers), false, "LAN Discovery" );
            window.Show();
        }

        void OnGUI()
        {
            // Possibly a memory leak but the GC seems to handle it
            ServerFoundEvent.serverFoundDel sfd = new ServerFoundEvent.serverFoundDel(Factors.LANServers.OnServerFound);

            if (Application.isPlaying)
            {
                // Register the Event Listener
                if (!isRegistered)
                {
                    ServerFoundEvent.OnServerFound += sfd;
                    Debug.Log("EDITOR_REG");
                    isRegistered = true;
                }

                // Show the recieved servers
                ServerDetails[] arr = Factors.LANServers.servers.ToArray();
                GUILayout.Label("Found " + arr.Length + ((arr.Length == 1) ? " server." : " servers."));
                foreach (ServerDetails i in Factors.LANServers.servers.ToArray())
                {
                    EditorGUILayout.HelpBox("IP: " + i.IP + ":" + i.Port + "\nFound via: " + i.DiscoveryMethod, MessageType.None);
                }
            } else
            {
                isRegistered = false;
                GUILayout.Label("Game not running!");
            }
        }

        void Update()
        {
            Repaint();
        }
    }
}