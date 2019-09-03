using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDMulti.LAN.Discovery.Providers
{
    public class Multicast
    {
        #region Dual

        private static IPEndPoint BroadcastEndpoint = new IPEndPoint(IPAddress.Parse("224.5.125.85"), 29571);

        #endregion

        #region Broadcast

        private static bool isBroadcasting = false;

        private static CancellationTokenSource BroadcastCts = new CancellationTokenSource();

        private static async Task BeginBroadcastingAsync(CancellationToken token)
        {
            byte[] buffer = new Message().Buffer();
            var client = new UdpClient(AddressFamily.InterNetwork);
            client.MulticastLoopback = true;
            client.JoinMulticastGroup(BroadcastEndpoint.Address);
            token.Register(() => client.Close());

            try
            {
                while (true)
                {
                    await client.SendAsync(buffer, buffer.Length, BroadcastEndpoint).ConfigureAwait(false);
                    await Task.Delay(TimeSpan.FromSeconds(1.5), token).ConfigureAwait(false);
                }
            }
            catch (ObjectDisposedException)
            {
                token.ThrowIfCancellationRequested();
                throw;
            }
            catch (TaskCanceledException)
            {
                // This occurs when you are waiting on a Task (in our case to wait 1.5 seconds) and it is cancelled. We can safely ignore this.
            }
        }

        public static async void StartBroadcasting()
        {
            if (isBroadcasting) return;
            isBroadcasting = true;
            Editor.Factors.ActiveItems.MulticastSend = isBroadcasting;
            await BeginBroadcastingAsync(BroadcastCts.Token);
            
        }

        public static void StopBroadcasting()
        {
            BroadcastCts.Cancel();
            isBroadcasting = false;
            Editor.Factors.ActiveItems.MulticastSend = isBroadcasting;
        }

        #endregion

        #region Receive

        private static bool isListening = false;

        private static CancellationTokenSource ListenCts = new CancellationTokenSource();

        private static async Task BeginListeningAsync(CancellationToken token)
        {
            var client = new UdpClient(BroadcastEndpoint.Port);
            client.JoinMulticastGroup(BroadcastEndpoint.Address);
            token.Register(() => client.Close());

            try
            {
                token.ThrowIfCancellationRequested();
                while (true)
                {
                    var result = await client.ReceiveAsync();
                    var data = Encoding.UTF8.GetString(result.Buffer);
                    if (data.StartsWith(new Message().Header(), StringComparison.Ordinal))
                    {
                        /*if (ServerFound != null)
                        {
                            var details = new ServerDetails
                            {
                                Hostname = result.RemoteEndPoint.Address.ToString(),
                                Port = int.Parse(data.Substring(Header.Length))
                            };
                            LoggingService.LogInfo("Found TunezServer at {0}", details.FullAddress);
                            ServerFound(this, details);
                        }*/
                        UnityEngine.Debug.Log("MF: " + data);
                        ServerFoundEvent.serverFoundDel sfd = new ServerFoundEvent.serverFoundDel(TEST);
                        ServerFoundEvent.OnServerFound += sfd;
                        string[] split = data.Split('/');
                        ServerFoundEvent.RegisterFoundServer(new ServerDetails()
                        {
                            IP = split[3].ToString(),
                            Port = int.Parse(split[4].ToString()),
                            DiscoveryMethod = DiscoveryMethod.Multicast
                        });
                        ServerFoundEvent.OnServerFound -= sfd;

                    }
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

        public static async void StartListening()
        {
            if (isListening) return;
            isListening = true;
            Editor.Factors.ActiveItems.MulticastRecv = isBroadcasting;
            await BeginListeningAsync(ListenCts.Token);

        }

        public static void StopListening()
        {
            ListenCts.Cancel();
            isListening = false;
            Editor.Factors.ActiveItems.MulticastRecv = isBroadcasting;
        }

        #endregion

        public static void TEST(ServerDetails sd)
        {
            UnityEngine.Debug.Log("LISTM");
        }
    }
}