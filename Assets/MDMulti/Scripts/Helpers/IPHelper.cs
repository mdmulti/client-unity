using System.Net;

namespace MDMulti
{
    public class IPHelper
    {
        public static byte[] ToBytes(string addr)
        {
            return IPAddress.Parse(addr).GetAddressBytes();
        }

        public static IPAddress ToAddressObject(byte[] b)
        {
            return new IPAddress(b);
        }
    }
}