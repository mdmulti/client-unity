using MDMulti.Net;

namespace MDMulti.LAN.Discovery
{
    public class PeerConnectionClient
    {
        public static void EnquireAboutPeer(ServerDetails sd)
        {
            GeneralSend gs = new GeneralSend(sd.GetIPEndPoint(), DataTypes.ReliableTCP);
            gs.Send("MDMPEER_ACK", res => {
                if (res != "MDMPEER_PROTO_1") throw new System.Exception("Unsupported Peer.");

                UnityEngine.Debug.Log("VALID PEER");
            });
        }
    }

    public class PeerConnectionServer
    {
        private GeneralRecieve gr;

        private UserFile hostPlayer;

        public int port { get; private set; }

        public PeerConnectionServer(int port, UserFile hostPlayer)
        {
            GeneralRecieve gr = new GeneralRecieve(port, DataTypes.ReliableTCP);
            this.gr = gr;
            this.port = port;
            this.hostPlayer = hostPlayer;
            gr.StartListening(onRecv);
        }

        public void Stop()
        {
            gr.StopListening();
        }

        private string onRecv(string data)
        {
            if (data.Equals("id")) return hostPlayer.ID;
            if (data.Equals("server_id")) return hostPlayer.ServerID;
            if (data.Equals("display_name")) return hostPlayer.DisplayName;

            if (data.Equals("MDMPEER_ACK")) return "MDMPEER_PROTO_1";

            return "MDMPEER_ERR_INVALID_MESSAGE";
        }
    }
}
