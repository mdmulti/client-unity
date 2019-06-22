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

        public static void Broadcast()
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
            Debug.Log("s: " + message.server);

            for (int i = 0; i <= 100; i++)
            {
                udpclient.Send(buffer, buffer.Length, remoteep);
                UnityEngine.Debug.Log("Sent " + i);
            }
        }

        //public static IEnumerator Search

        [Serializable]
        public class Message
        {
            public string server;
            public int protocolVersion;
            public string applicationName;

            public Message()
            {
                server = "MDMulti";
                protocolVersion = Rest.ProtocolVersion;
            }
        }
    }
}