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

        private UdpClient udp;
        
        public PlainClient(IPEndPoint ipep)
        {
            this.ipep = ipep;
            Setup();

        }

        public async Task<string> Send(string sdata)
        {
            // Convert the data to UTF8 and escape it
            byte[] data = AddInfoData(Encoding.ASCII.GetBytes(EscapeHelper.B64Escape(sdata)));

            // Send the data
            udp.Send(data, data.Length);

            // Recieve data back from the server
            // Buffer to store the response bytes.
            var res = await udp.ReceiveAsync();
            byte[] responseDataBytes = res.Buffer;

            string responseData = Encoding.ASCII.GetString(responseDataBytes, 0, responseDataBytes.Length);

            // Parse the response (to make sure it's valid data) and send the actual message
            return ParseResponse(responseData);
        }

        private void Setup()
        {
            // Create the TCP client
            UdpClient udp = new UdpClient();

            // Disallow exclusive address/port bind use.
            udp.ExclusiveAddressUse = false;

            // Attempt to connect to the server / destination
            try
            {
                udp.Connect(ipep);
            }
            catch (SocketException ex)
            {
                throw new DataConnectionRefused("The TCP connection to " + ipep.ToString() + " has been refused.", ex);
            }

            // The connnection passed

            // Set variables
            this.udp = udp;
        }
    }
}