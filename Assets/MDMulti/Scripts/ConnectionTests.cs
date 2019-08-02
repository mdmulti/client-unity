using System.Net.Sockets;
using LumiSoft.Net.STUN.Client;

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
            STUN_Result result = STUN_Client.Query("stun.l.google.com", 19302, s);
            return result.NetType.ToString();
        }
    }
}