using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace MDMulti.LAN.Discovery.Providers
{
    public class Broadcast
    {
        private static readonly int PORT = 25816;

        public static Opts Setup()
        {
            UdpClient udpclient = new UdpClient();
            udpclient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            byte[] buffer = new Message().Buffer();

            return new Opts(udpclient, buffer);
        }

        private static IEnumerator Send(UdpClient udpclient, byte[] buffer)
        {
            while (true)
            {
                udpclient.Send(buffer, buffer.Length, "255.255.255.255", PORT);
                Debug.Log("Sent Broadcast on frame " + Time.frameCount);
                yield return Core.waitForSeconds;
            }
        }

        private static bool Active = false;
        private static Coroutine coroutineInstance;

        public static void Start(Opts o)
        {
            if (!Active)
            {
                coroutineInstance = Mono.Main.Inst.StartCoroutine(Send(o.udpclient, o.buffer));
                Active = true;
            }

            EditorExternalFactors.BroadcastActive = Active;
        }

        public static void Stop(Opts o)
        {
            if (Active)
            {
                // Stop the routine
                Mono.Main.Inst.StopCoroutine(coroutineInstance);

                // Close the connection, freeing the port
                o.udpclient.Close();

                // Set the boolean
                Active = false;
            }

            EditorExternalFactors.BroadcastActive = Active;
        }

        [Serializable]
        public class Opts
        {
            public UdpClient udpclient;
            public Byte[] buffer;

            public Opts(UdpClient udpclient, Byte[] buffer)
            {
                this.udpclient = udpclient;
                this.buffer = buffer;
            }
        }
    }
}