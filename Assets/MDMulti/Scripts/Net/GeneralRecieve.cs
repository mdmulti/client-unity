using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MDMulti.Net
{
    public class GeneralRecieve : GeneralCore
    {
        // Maybe change this to a port only?
        private IPEndPoint ipep;
        private DataTypes dt;

        private UdpClient udp;
        private TcpListener tcp;

        private bool isListening = false;
        private CancellationTokenSource udpCts = new CancellationTokenSource();

        public GeneralRecieve(IPEndPoint ipep, DataTypes dt)
        {
            this.ipep = ipep;
            this.dt = dt;

            Setup();

        }

        private void Setup()
        {
            switch (dt)
            {
                case DataTypes.UnreliableUDP:
                    if (udp == null)
                    {
                        UdpClient ludp = new UdpClient();
                        udp = ludp;
                    }
                    break;
                case DataTypes.ReliableTCP:
                    if (tcp == null)
                    {
                        // Create the TCP client
                        TcpListener ltcp = new TcpListener(ipep);

                        ltcp.Start();

                        // Set variables
                        tcp = ltcp;
                    }
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        private async Task ListenUDP(CancellationToken token)
        {
            var client = new UdpClient(ipep);
            token.Register(() => client.Close());

            try
            {
                token.ThrowIfCancellationRequested();
                while(true)
                {
                    UdpReceiveResult res = await client.ReceiveAsync();
                    string data = Encoding.UTF8.GetString(res.Buffer);

                    if (data.StartsWith(GetInfoData(), StringComparison.Ordinal))
                    {
                        UnityEngine.Debug.Log("GR DATA: " + data);
                    }
                }
            } catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }
        }

        public async void StartListening()
        {
            if (isListening) return;
            isListening = true;

            await ListenUDP(udpCts.Token);
        }

        public void StopListening()
        {
            if (!isListening) return;
            isListening = false;

            udpCts.Cancel();
        }
    }
}