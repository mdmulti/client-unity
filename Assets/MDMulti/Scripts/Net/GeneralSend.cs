using UnityEngine;
using UnityEditor;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MDMulti.Net
{
    public class GeneralSend : GeneralCore
    {
        private IPEndPoint ipep;
        private DataTypes dt;

        private UdpClient udp;

        private TcpClient tcp;
        private NetworkStream tcp_netstream;
        
        public GeneralSend(IPEndPoint ipep, DataTypes dt)
        {
            this.ipep = ipep;
            this.dt = dt;

            Setup();

        }

        public void Send(string data)
        {
            // Convert the data to UTF8 and escape it
            byte[] bdata = AddInfoData(Encoding.UTF8.GetBytes(EscapeHelper.Escape(data)));

            switch (dt)
            {
                case DataTypes.UnreliableUDP:
                    SendUDP(bdata);
                    break;
                //case DataTypes.ReliableTCP:
                //    SendTCP(ipep, bdata);
                //    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        private void SendUDP(byte[] data)
        {
            udp.Send(data, data.Length, ipep);
        }

        private void SendTCP(byte[] data)
        {
            tcp_netstream.Write(data, 0, data.Length);
        }

        private void Setup()
        {
            switch (dt)
            {
                case DataTypes.UnreliableUDP:
                    if (udp == null)
                    {
                        UdpClient ludp = new UdpClient();
                        udp = ludp;
                    }
                    break;
                case DataTypes.ReliableTCP:
                    if (tcp == null)
                    {
                        // Create the TCP client
                        TcpClient ltcp = new TcpClient();

                        // Disallow exclusive address/port bind use.
                        ltcp.ExclusiveAddressUse = false;

                        // Attempt to connect to the server / destination
                        try
                        {
                            ltcp.Connect(ipep);
                        } catch (SocketException ex)
                        {
                            throw new DataConnectionRefused("The TCP connection to " + ipep.ToString() + " has been refused.", ex);
                        }

                        // The connnection passed

                        // Set variables
                        tcp = ltcp;
                        tcp_netstream = tcp.GetStream();
                    }
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}