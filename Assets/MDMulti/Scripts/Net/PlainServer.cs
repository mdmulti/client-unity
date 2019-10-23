using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MDMulti.Net
{
    public class PlainServer : Core
    {
        private int port;

        private UdpClient udp;

        protected bool isListening = false;
        protected CancellationTokenSource cts = new CancellationTokenSource();

        public PlainServer(int port)
        {
            this.port = port;

            Setup();

        }

        private void Setup()
        {
            if (udp == null)
            {
                // Create the TCP client
                UdpClient udp = new UdpClient(port);

                // Set variables
                this.udp = udp;
            }
        }

        protected async Task ListenUDP(CancellationToken token, Func<string, string> onReceive)
        {
            token.Register(() => udp.Close());

            try
            {
                token.ThrowIfCancellationRequested();
                while (true)
                {
                    var result = await udp.ReceiveAsync();
                    var data = Encoding.UTF8.GetString(result.Buffer);
                    byte[] res = AddInfoData(Encoding.ASCII.GetBytes(EscapeHelper.B64Escape(onReceive(ParseResponse(data)))));
                    await udp.SendAsync(res, res.Length, result.RemoteEndPoint);
                }
            }
            catch (ObjectDisposedException)
            {
                // This will happen when we cancel the task.  We can ignore this.
            }
            catch (SocketException)
            {
                token.ThrowIfCancellationRequested();
                // Ignore this
            }
            catch (Exception ex)
            {
                token.ThrowIfCancellationRequested();
                UnityEngine.Debug.Log("Ignoring bad UDP " + ex);
            }
        }

        public async void StartListening(Func<string, string> onRecv)
        {
            if (isListening) return;
            isListening = true;
            await ListenUDP(cts.Token, onRecv);
        }

        public void StopListening()
        {
            if (!isListening) return;
            isListening = false;

            cts.Cancel();
        }
    }
}