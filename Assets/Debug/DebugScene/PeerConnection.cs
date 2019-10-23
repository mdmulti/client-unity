using MDMulti.LAN.Discovery;
using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class PeerConnection : MonoBehaviour
    {

        private ServerFoundEvent.serverFoundDel sfd;

        public void AddListener()
        {
            sfd = new ServerFoundEvent.serverFoundDel(onServerFoundTest);
            ServerFoundEvent.OnServerFound += sfd;
        }

        private async void onServerFoundTest(ServerDetails sfe)
        {
            Debug.LogError("L_INT");
            Debug.LogError(await new PeerConnectionClient(sfe).IsValidPeer());
            Debug.LogError("L_INT_REM_INPROG");
            ServerFoundEvent.OnServerFound -= sfd;
            Debug.LogError("L_INT_REM_DONE");
        }
    }
}