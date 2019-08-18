using System;
using System.Text;
using UnityEngine;

namespace MDMulti.LAN.Discovery
{
    /// <summary>
    /// Class for creating Multicast / Broadcast messages.
    /// </summary>
    [Serializable]
    public class Message
    {
        public string server;
        public int protocolVersion;
        public string escapedApplicationName;
        // There is no point temp storing the IP here as an IPAddress as it will be sent as a string anyway.
        public string ip;
        public int port;

        /// <summary>
        /// Constructor for creating Multicast / Broadcast messages.
        /// </summary>
        public Message()
        {
            server = "MDMulti";
            protocolVersion = Rest.ProtocolVersion;
            escapedApplicationName = EscapeHelper.Escape(Mono.Options.Instance.appName);
            ip = ConnectionTests.GetLANIP().ToString();
            port = 69;
        }

        public byte[] Buffer()
        {
           return Encoding.UTF8.GetBytes(server + "/" + protocolVersion + "/" + escapedApplicationName + "/" + ip + "/" + port);
        }

        public string Header()
        {
            return server + "/" + protocolVersion + "/" + escapedApplicationName;
        }
    }
}