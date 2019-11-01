using UnityEngine;
using MDMulti.Constants;

namespace MDMulti.LAN.Discovery
{
    public class Core
    {
        protected static Lan constants = ConstantsHelper.Get().Lan;

        public static WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

        private static PeerConnectionServer pcs;

        protected static bool isPcs_Multicast = false;
        protected static bool isPcs_Broadcast = false;

        protected static void StartPcsIfNeeded(UserFile hostPlayer)
        {
            if (pcs == null)
            {
                UnityEngine.Debug.LogError("STARTING PCS");
                pcs = new PeerConnectionServer(PortHelper.GetRandomAvailablePort(), hostPlayer);
            }
        }

        protected static void StopPcsIfNeeded()
        {
            if (pcs != null && !isPcs_Multicast && !isPcs_Broadcast)
            {
                UnityEngine.Debug.LogError("STOPPING PCS");
                pcs.Stop();
                pcs = null;
            }
        }

        protected static int GetPcsPort()
        {
            return pcs.port;
        }
    }
}