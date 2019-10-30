using MDMulti;
using MDMulti.LAN.Discovery.Providers;
using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class LANDiscovery : MonoBehaviour
    {
        public async void MulticastStart()
        {
            Multicast.StartBroadcasting(new UserFile(await CertHelper.GetCertificateFromFile("p4.crt")));
        }

        public void MulticastStop()
        {
            Multicast.StopBroadcasting();
        }

        public void MulticastRXStart()
        {
            Multicast.StartListening();
        }

        public void MulticastRXStop()
        {
            Multicast.StopListening();
        }

        public async void BroadcastStart()
        {
            Broadcast.StartBroadcasting(new UserFile(await CertHelper.GetCertificateFromFile("p4.crt")));
        }

        public void BroadcastStop()
        {
            Broadcast.StopBroadcasting();
        }

        public void BroadcastRXStart()
        {
            Broadcast.StartListening();
        }

        public void BroadcastRXStop()
        {
            Broadcast.StopListening();
        }
    }
}