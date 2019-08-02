using System.Net.Sockets;

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
    }
}