using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace MDMulti.LAN.Discovery.Providers
{
    public class Multicast
    {
        public static readonly string MulticastAddressString = Mono.Options.Instance.multicast.ip;

        public static Opts Setup()
        {
            UdpClient udpclient = new UdpClient();

            IPAddress multicastaddress = IPAddress.Parse(MulticastAddressString);
            udpclient.JoinMulticastGroup(multicastaddress);
            IPEndPoint remoteep = new IPEndPoint(multicastaddress, Mono.Options.Instance.multicast.port);

            Byte[] buffer = new Message().Buffer();

            return new Opts(udpclient, buffer, remoteep);
        }

        private static IEnumerator Send(UdpClient udpclient, Byte[] buffer, IPEndPoint remoteep)
        {
            while (true) {
                udpclient.Send(buffer, buffer.Length, remoteep);
                Debug.Log("Sent Multicast on frame " + Time.frameCount);
                yield return Core.waitForSeconds;
            }
        }

        private static bool isBroadcasting = false;
        private static Coroutine coroutineInstance;

        public static void Start(Opts o)
        {
            if (!isBroadcasting)
            {
                coroutineInstance = Mono.Main.Inst.StartCoroutine(Send(o.udpclient, o.buffer, o.remoteep));
                isBroadcasting = true;
            }

            EditorExternalFactors.MulticastActive = isBroadcasting;
        }

        public static void Stop()
        {
            if (isBroadcasting)
            {
                Mono.Main.Inst.StopCoroutine(coroutineInstance);
                isBroadcasting = false;
            }

            EditorExternalFactors.MulticastActive = isBroadcasting;
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
    }
}