using System.Net.Sockets;

using MDMulti.STUN.Client;

namespace MDMulti
{
    public class ConnectionTests
    {
        public static bool WAN()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                /* 
                 * Google Public DNS
                 * (google-public-dns-a.google.com)
                 * Open on 53/tcp (DNS)
                 */
                s.Connect("8.8.8.8", 53);
                return s.Connected;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public static string NAT()
        {
            // Create new socket for STUN client.
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Query STUN server
            Result result = Client.Query("stun.l.google.com", 19302, s);

            // Return a data string
            return result.NetType.ExtendedMessage;
        }

        public static string WANIP()
        {
            // Create new socket for STUN client.
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Query STUN server
            Result result = Client.Query("stun.l.google.com", 19302, s);

            string ip;
            // If the PublicEndPoint (public IP / port used for the connection) is null
            if (result.PublicEndPoint == null)
            {
                ip = "Not Available";
            } else
            {
                ip = result.PublicEndPoint.Address.ToString();
            }

            // Return the data
            return ip;
        }
    }
}