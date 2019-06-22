using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace MDMulti
{
    public class Multicast
    {
        public static readonly string MulticastAddressString = "224.5.125.85";

        private static WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

        public static Opts SetupForBroadcast()
        {
            UdpClient udpclient = new UdpClient();

            IPAddress multicastaddress = IPAddress.Parse(MulticastAddressString);
            udpclient.JoinMulticastGroup(multicastaddress);
            IPEndPoint remoteep = new IPEndPoint(multicastaddress, 29571);

            Byte[] buffer = null;

            Message message = new Message();
            message.applicationName = "Test";
            buffer = Encoding.Unicode.GetBytes(JsonUtility.ToJson(message));

            Debug.Log("Message: " + JsonUtility.ToJson(message));

            return new Opts(udpclient, buffer, remoteep);
        }

        private static IEnumerator Broadcast(UdpClient udpclient, Byte[] buffer, IPEndPoint remoteep)
        {
            while (true) {
                udpclient.Send(buffer, buffer.Length, remoteep);
                Debug.Log("Sent " + Time.frameCount);
                yield return waitForSeconds;
            }
        }

        private static bool isBroadcasting = false;
        private static Coroutine coroutineInstance;

        public static void StartBroadcasting(Opts o)
        {
            if (!isBroadcasting)
            {
                coroutineInstance = MainMono.Mono.StartCoroutine(Broadcast(o.udpclient, o.buffer, o.remoteep));
                isBroadcasting = true;
            }

            EditorExternalFactors.MulticastBroadcastActive = isBroadcasting;
        }

        public static void StopBroadcasting()
        {
            if (isBroadcasting)
            {
                MainMono.Mono.StopCoroutine(coroutineInstance);
                isBroadcasting = false;
            }

            EditorExternalFactors.MulticastBroadcastActive = isBroadcasting;
        }

        [Serializable]
        public class Opts
        {
            public UdpClient udpclient;
            public Byte[] buffer;
            public IPEndPoint remoteep;

            public Opts(UdpClient udpclient, Byte[] buffer, IPEndPoint remoteep)
            {
                this.udpclient = udpclient;
                this.buffer = buffer;
                this.remoteep = remoteep;
            }
        }

        /// <summary>
        /// Class for creating Multicast messages.
        /// </summary>
        [Serializable]
        public class Message
        {
            public string server;
            public int protocolVersion;
            public string applicationName;

            /// <summary>
            /// Constructor for creating Multicast messages.
            /// </summary>
            public Message()
            {
                server = "MDMulti";
                protocolVersion = Rest.ProtocolVersion;
            }
        }
    }
}