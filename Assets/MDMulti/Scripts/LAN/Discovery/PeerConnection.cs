using MDMulti.Net;
using System.Threading.Tasks;

namespace MDMulti.LAN.Discovery
{
    public class PeerConnectionClient
    {
        /*public static void EnquireAboutPeer(ServerDetails sd)
        {
            PlainClient gs = new PlainClient(sd.GetIPEndPoint());
            gs.Send("MDMPEER_ACK", res => {
                if (res != "MDMPEER_PROTO_1") throw new System.Exception("Unsupported Peer.");

                UnityEngine.Debug.Log("VALID PEER");
            });
        }*/

        private ServerDetails sd;
        private PlainClient ps;

        public readonly string ProtocolVersionString = "MDMPEER_PROTO_1";

        public PeerConnectionClient(ServerDetails sd)
        {
            this.sd = sd;
            this.ps = new PlainClient(sd.GetIPEndPoint());
        }

        public bool IsValidPeer()
        {
            //return (await ps.Send("MDMPEER_ACK")) == ProtocolVersionString;
            string res =  ps.Send("MDMPEER_ACK");

            UnityEngine.Debug.LogError("PEERCONN_C_RES = " + res);

            return res == ProtocolVersionString;
        }
    }

    public class PeerConnectionServer
    {
        private PlainServer ps;

        private UserFile hostPlayer;

        public int port { get; private set; }

        public PeerConnectionServer(int port, UserFile hostPlayer)
        {
            PlainServer ps = new PlainServer(port);
            this.ps = ps;
            this.port = port;
            this.hostPlayer = hostPlayer;
            ps.StartListening(onRecv);
        }

        public void Stop()
        {
            ps.StopListening();
        }

        private string onRecv(string data)
        {
            UnityEngine.Debug.LogError("PEERCONN_S_ONRECV = " + data);
            if (data.Equals("id")) return hostPlayer.ID;
            if (data.Equals("server_id")) return hostPlayer.ServerID;
            if (data.Equals("display_name")) return hostPlayer.DisplayName;

            if (data.Equals("MDMPEER_ACK")) return "MDMPEER_PROTO_1";

            return "MDMPEER_ERR_INVALID_MESSAGE";
        }
    }
}
