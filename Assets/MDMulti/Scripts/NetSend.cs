using UnityEngine;
using UnityEditor;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MDMulti
{
    public class NetSend
    {
        /*
        private IPEndPoint ipep;
        
        public NetSend(IPEndPoint ipep)
        {
            this.ipep = ipep;
        }
        */

        public static void Send(IPEndPoint ipep, DataTypes dt, string data)
        {
            // Convert the data to UTF8 and escape it
            byte[] bdata = Encoding.UTF8.GetBytes(EscapeHelper.Escape(data));

            switch (dt)
            {
                case DataTypes.UnreliableUDP:
                    SendUDP(ipep, bdata);
                    break;
                default:
                    throw new System.Exception();
            }
        }

        private static void SendUDP(IPEndPoint ipep, byte[] bdata)
        {
            UdpClient client = new UdpClient();
            client.Send(bdata, bdata.Length, ipep);
        }

        public enum DataTypes
        {
            ReliableTCP = 0,
            UnreliableUDP = 1,
        }
    }
}