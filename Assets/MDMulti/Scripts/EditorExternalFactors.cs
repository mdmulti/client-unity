using MDMulti.LAN.Discovery;
using System.Collections.Generic;

namespace MDMulti
{
    public class EditorExternalFactors
    {
        public static bool MulticastActive = false;
        public static bool BroadcastActive = false;
    }
}

namespace MDMulti.Editor.Factors
{
    public class LANServers
    {
        public static List<ServerDetails> servers = new List<ServerDetails>();
        
        public static void OnServerFound(ServerDetails sd)
        {
            if (!servers.Contains(sd)) servers.Add(sd);
            UnityEngine.Debug.Log("EDITOR_FACTORS_IP " + sd.IP);
            UnityEngine.Debug.Log("EDITOR_FACTORS " + servers.ToArray().Length);
        }
    }
}