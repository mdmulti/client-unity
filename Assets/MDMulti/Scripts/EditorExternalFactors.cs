using MDMulti.LAN.Discovery;
using System.Collections.Generic;

namespace MDMulti.Editor.Factors
{
    public class ActiveItems
    {
        public static bool MulticastSend = false;
        public static bool MulticastRecv = false;
        public static bool BroadcastSend = false;
        public static bool BroadcastRecv = false;
    }

    public class LANServers
    {
        public static List<ServerDetails> servers = new List<ServerDetails>();
        
        public static void OnServerFound(ServerDetails sd)
        {
            UnityEngine.Debug.Log("EDITOR_FACTORS_IP " + sd.IP);
            UnityEngine.Debug.Log("EDITOR_FACTORS " + servers.ToArray().Length);

            // If the server does not exist in the list whatsoever then add it
            if (!servers.Contains(sd)) {
                servers.Add(sd);
                UnityEngine.Debug.Log("A1");
            } else
            // The server exists in the list, so we will check to see if the DiscoveryMethod matches
            {
                ServerDetails found = null;
                UnityEngine.Debug.Log("A2");

                found = servers.Find(s => s.IP == sd.IP && s.Port == sd.Port && s.DiscoveryMethod != sd.DiscoveryMethod);


                UnityEngine.Debug.Log("A2:1 " + found);
                // The check
                if (found != null)
                {
                    UnityEngine.Debug.Log("A3");
                    // We got a match!
                    // Now we can delete the old server details and add the updated version!
                    servers.Remove(found);
                    found.DiscoveryMethod = DiscoveryMethod.MultipleMethods;
                    servers.Add(found);
                }

            }
        }
    }
}