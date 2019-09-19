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
                    throw new System.NotImplementedException();
            }
        }

        private static void SendUDP(IPEndPoint ipep, byte[] data)
        {
            UdpClient client = new UdpClient();
            byte[] bdata = AddInfoData(data);
            client.Send(bdata, bdata.Length, ipep);
        }

        private static byte[] AddInfoData(byte[] data)
        {
            LAN.Discovery.Message m = new LAN.Discovery.Message();
            string h = m.server + "/" + m.escapedApplicationName + "/////";

            byte[] headerb = Encoding.UTF8.GetBytes(h);

            byte[] res = new byte[data.Length + headerb.Length];
            System.Buffer.BlockCopy(headerb, 0, res, 0, headerb.Length);
            System.Buffer.BlockCopy(data, 0, res, headerb.Length, data.Length);

            return res;

        }

        public enum DataTypes
        {
            ReliableTCP = 0,
            UnreliableUDP = 1,
        }
    }
}