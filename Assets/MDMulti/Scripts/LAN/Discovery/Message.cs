using System;
using System.Text;
using UnityEngine;

namespace MDMulti.LAN.Discovery
{
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
            applicationName = Mono.Options.Instance.appName;
        }

        public byte[] Buffer()
        {
           return Encoding.Unicode.GetBytes(JsonUtility.ToJson(this));
        }
    }
}