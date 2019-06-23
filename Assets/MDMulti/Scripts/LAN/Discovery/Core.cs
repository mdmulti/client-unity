using UnityEngine;

namespace MDMulti.LAN.Discovery
{
    public class Core
    {
        public static WaitForSeconds waitForSeconds = new WaitForSeconds(Mono.Options.Instance.multicast.broadcastDelay);
    }
}