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
        private static bool isBroadcasting = false;

        private static CancellationTokenSource BroadcastCts = new CancellationTokenSource();

        private static IPEndPoint BroadcastEndpoint = new IPEndPoint(IPAddress.Parse("224.5.125.85"), 29571);

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
            EditorExternalFactors.MulticastActive = isBroadcasting;
            await BeginBroadcastingAsync(BroadcastCts.Token);
            
        }

        public static void StopBroadcasting()
        {
            BroadcastCts.Cancel();
            isBroadcasting = false;
            EditorExternalFactors.MulticastActive = isBroadcasting;
        }

        #region Receive

        public static async Task BeginListeningAsync(CancellationToken token)
        {
            var client = new UdpClient(BroadcastEndpoint.Port);
            client.JoinMulticastGroup(BroadcastEndpoint.Address);
            token.Register(() => client.Close());

            while (true)
            {
                token.ThrowIfCancellationRequested();
                try
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

                    }
                }
                catch (ObjectDisposedException)
                {
                    token.ThrowIfCancellationRequested();
                    throw;
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
        }

        #endregion
    }
}