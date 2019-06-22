using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MDMulti
{
    public class Multicast
    {
        public static readonly string MulticastAddressString = "224.5.125.85";

        public static void Connect()
        {
            UdpClient udpclient = new UdpClient();

            IPAddress multicastaddress = IPAddress.Parse(MulticastAddressString);
            udpclient.JoinMulticastGroup(multicastaddress);
            IPEndPoint remoteep = new IPEndPoint(multicastaddress, 29571);

            Byte[] buffer = null;
 
            for (int i = 0; i <= 800; i++)
            {
                buffer = Encoding.Unicode.GetBytes(i.ToString());
                udpclient.Send(buffer, buffer.Length, remoteep);
                UnityEngine.Debug.Log("Sent " + i);
            }
        }
    }
}