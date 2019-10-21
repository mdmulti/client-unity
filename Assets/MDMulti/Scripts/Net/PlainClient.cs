using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading.Tasks;

namespace MDMulti.Net
{
    /// <summary>
    /// A Plain-Text TCP client.
    /// Mainly used for LAN Peer discovery.
    /// </summary>
    public class PlainClient : Core
    {
        private IPEndPoint ipep;

        private TcpClient tcp;
        private NetworkStream tcp_netstream;
        
        public PlainClient(IPEndPoint ipep)
        {
            this.ipep = ipep;
            Setup();

        }

        public string Send(string sdata)
        {
            // Convert the data to UTF8 and escape it
            byte[] data = AddInfoData(Encoding.UTF8.GetBytes(EscapeHelper.Escape(sdata)));

            // Send the data
            tcp_netstream.Write(data, 0, data.Length);

            // Recieve data back from the server
            // Buffer to store the response bytes.
            byte[] responseDataBytes = new byte[256];

            // Read the first batch of the TcpServer response bytes.
            int bytes = tcp_netstream.Read(responseDataBytes, 0, responseDataBytes.Length);
            string responseData = Encoding.UTF8.GetString(responseDataBytes, 0, bytes);

            // Parse the response (to make sure it's valid data) and send the actual message
            return ParseResponse(responseData);
        }

        private void Setup()
        {
            // Create the TCP client
            TcpClient ltcp = new TcpClient();

            // Disallow exclusive address/port bind use.
            ltcp.ExclusiveAddressUse = false;

            // Attempt to connect to the server / destination
            try
            {
                ltcp.Connect(ipep);
            }
            catch (SocketException ex)
            {
                throw new DataConnectionRefused("The TCP connection to " + ipep.ToString() + " has been refused.", ex);
            }

            // The connnection passed

            // Set variables
            tcp = ltcp;
            tcp_netstream = tcp.GetStream();
        }
    }
}