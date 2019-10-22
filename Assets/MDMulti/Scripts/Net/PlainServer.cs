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

        private TcpListener tcp;

        protected bool isListening = false;
        protected CancellationTokenSource cts = new CancellationTokenSource();

        public PlainServer(int port)
        {
            this.port = port;

            Setup();

        }

        private void Setup()
        {
            if (tcp == null)
            {
                // Create the TCP client
                TcpListener ltcp = new TcpListener(IPAddress.Any, port);

                //ltcp.Start();

                // Set variables
                tcp = ltcp;
            }
        }

        protected async Task ListenTCP(CancellationToken token, Func<string, string> onReceive)
        {
            token.Register(() => tcp.Stop());

            // Buffer to store the response bytes.
            byte[] bytes = new byte[1024];

            try
            {
                token.ThrowIfCancellationRequested();
                while (true)
                {
                    // Start Listening for requests
                    tcp.Start();

                    // Accept an incoming connection
                    TcpClient client = await tcp.AcceptTcpClientAsync();

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    // Counter
                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a UTF8 string.
                        string data = Encoding.ASCII.GetString(bytes, 0, i);
                        UnityEngine.Debug.Log("TCPGR Received: " + data);

                        // Process the data sent by the client (call the specified function)
                        string result_raw = onReceive(ParseResponse(data));

                        // Add the MDMPEER header
                        string result = GetInfoData() + EscapeHelper.B64Escape(result_raw);

                        byte[] msg = Encoding.ASCII.GetBytes(result);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        UnityEngine.Debug.Log("TCPGR Sent: " + data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }
        }

        public async void StartListening(Func<string, string> onRecv)
        {
            if (isListening) return;
            isListening = true;
            await ListenTCP(cts.Token, onRecv);
        }

        public void StopListening()
        {
            if (!isListening) return;
            isListening = false;

            cts.Cancel();
        }
    }
}