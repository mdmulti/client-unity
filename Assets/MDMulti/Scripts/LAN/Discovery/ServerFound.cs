using System;
using System.Net;

namespace MDMulti.LAN.Discovery
{
    public class ServerFoundEvent
    {
        public delegate void serverFoundDel(ServerDetails sfe);
        public static event serverFoundDel OnServerFound;

        public static void RegisterFoundServer(ServerDetails sd)
        {
            if (OnServerFound != null) OnServerFound(sd);
        }
    }

    public enum DiscoveryMethod
    {
        Broadcast = 0,
        Multicast = 1,
        MultipleMethods = 9
    };

    public class ServerDetails : IEquatable<ServerDetails>
    {
        public string IP
        {
            get; set;
        }

        public int Port
        {
            get; set;
        }

        public DiscoveryMethod DiscoveryMethod
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ServerDetails);
        }

        public bool Equals(ServerDetails other)
        {
            return other != null && other.IP == IP && other.Port == Port;
        }

        public override int GetHashCode()
        {
            return IP.GetHashCode();
        }

        public IPEndPoint GetIPEndPoint()
        {
            return new IPEndPoint(IPHelper.ToAddressObject(IPHelper.ToBytes(IP)), Port);
        }
    }
}