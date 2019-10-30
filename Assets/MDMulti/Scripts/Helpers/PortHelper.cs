
using System.Linq;
using System.Net.NetworkInformation;

namespace MDMulti
{
    class PortHelper
    {
        public static readonly int dynamicStartingPort = 49152;

        // Original code from https://gist.github.com/jrusbatch/4211535
        public static int GetRandomAvailablePort()
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            var tcpConnectionPorts = properties.GetActiveTcpConnections()
                                .Where(n => n.LocalEndPoint.Port >= dynamicStartingPort)
                                .Select(n => n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            var tcpListenerPorts = properties.GetActiveTcpListeners()
                                .Where(n => n.Port >= dynamicStartingPort)
                                .Select(n => n.Port);

            //getting active udp listeners
            var udpListenerPorts = properties.GetActiveUdpListeners()
                                .Where(n => n.Port >= dynamicStartingPort)
                                .Select(n => n.Port);

            var port = Enumerable.Range(dynamicStartingPort, ushort.MaxValue)
                .Where(i => !tcpConnectionPorts.Contains(i))
                .Where(i => !tcpListenerPorts.Contains(i))
                .Where(i => !udpListenerPorts.Contains(i))
                .FirstOrDefault();

            UnityEngine.Debug.Log("PHP: " + port);
            return port;
        }
    }
}
