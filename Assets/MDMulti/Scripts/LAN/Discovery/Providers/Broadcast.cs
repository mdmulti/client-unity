using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MDMulti.LAN.Discovery.Providers
{
    public class Broadcast
    {
        private static bool isBroadcasting = false;

        private static CancellationTokenSource BroadcastCts = new CancellationTokenSource();

        private static IPEndPoint BroadcastEndpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 25816);

        private static async Task BeginBroadcastingAsync(CancellationToken token)
        {
            byte[] buffer = new Message().Buffer();
            var client = new UdpClient(AddressFamily.InterNetwork);
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
            EditorExternalFactors.BroadcastActive = isBroadcasting;
            await BeginBroadcastingAsync(BroadcastCts.Token);

        }

        public static void StopBroadcasting()
        {
            BroadcastCts.Cancel();
            isBroadcasting = false;
            EditorExternalFactors.BroadcastActive = isBroadcasting;
        }
    }
}