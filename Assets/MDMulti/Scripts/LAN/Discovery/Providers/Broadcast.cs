﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDMulti.LAN.Discovery.Providers
{
    public class Broadcast : Core
    {
        #region Dual

        private static IPEndPoint BroadcastEndpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), constants.Broadcast.Port);

        #endregion

        #region Broadcast

        private static PeerConnectionServer pcs;

        private static bool isBroadcasting = false;

        private static CancellationTokenSource BroadcastCts = new CancellationTokenSource();
        
        private static async Task BeginBroadcastingAsync(CancellationToken token, int port)
        {
            byte[] buffer = new Message(port).Buffer();
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

        public static async void StartBroadcasting(UserFile hostPlayer)
        {
            // Make sure we aren't already broadcasting
            if (isBroadcasting) return;

            // Set some variables for later
            isBroadcasting = true;
            Editor.Factors.ActiveItems.BroadcastSend = isBroadcasting; // This is used for the custom editor windows.

            // Start the PeerConnectionServer if it hasn't been already
            isPcs_Broadcast = true;
            StartPcsIfNeeded(hostPlayer);

            // Start broadcasting
            await BeginBroadcastingAsync(BroadcastCts.Token, GetPcsPort());
        }

        public static void StopBroadcasting()
        {
            // Cancel the token, this stops the broadcast loop
            BroadcastCts.Cancel();

            // Set the 'are we using PCS' variable to false
            isPcs_Broadcast = false;

            // If no other providers are using PCS, stop the server.
            StopPcsIfNeeded();

            // Set some other variables
            isBroadcasting = false;
            Editor.Factors.ActiveItems.BroadcastSend = isBroadcasting;
        }

        #endregion

        #region Receive

        private static bool isListening = false;

        private static CancellationTokenSource ListenCts = new CancellationTokenSource();

        private static async Task BeginListeningAsync(CancellationToken token)
        {
            var client = new UdpClient(BroadcastEndpoint.Port);
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
                        UnityEngine.Debug.Log("BF: " + data);
                        //ServerFoundEvent.serverFoundDel sfd = new ServerFoundEvent.serverFoundDel(TEST);
                        //ServerFoundEvent.OnServerFound += sfd;
                        string[] split = data.Split('/');
                        ServerFoundEvent.RegisterFoundServer(new ServerDetails()
                        {
                            IP = split[3].ToString(),
                            Port = int.Parse(split[4].ToString()),
                            DiscoveryMethod = DiscoveryMethod.Broadcast
                        });
                        //ServerFoundEvent.OnServerFound -= sfd;

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
            Editor.Factors.ActiveItems.BroadcastRecv = isBroadcasting;
            await BeginListeningAsync(ListenCts.Token);

        }

        public static void StopListening()
        {
            ListenCts.Cancel();
            isListening = false;
            Editor.Factors.ActiveItems.BroadcastRecv = isBroadcasting;
        }

        #endregion
    }
}