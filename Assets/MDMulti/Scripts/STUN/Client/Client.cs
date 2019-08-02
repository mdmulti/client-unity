// Original project copyright (c) Ivar Lumi 2007
// Licensed under CPOL. See CPOL.html for more details.
// https://www.codeproject.com/Articles/18492/STUN-Client

using System;
using System.Net;
using System.Net.Sockets;

using MDMulti.STUN.Message;

namespace MDMulti.STUN.Client
{
    /// <summary>
    /// This class implements STUN client. Defined in RFC 3489.
    /// </summary>
    public class Client
    {
        #region static method Query

        /// <summary>
        /// Gets NAT info from STUN server.
        /// </summary>
        /// <param name="host">STUN server name or IP.</param>
        /// <param name="port">STUN server port. Default port is 3478.</param>
        /// <param name="socket">UDP socket to use.</param>
        /// <returns>Returns UDP netwrok info.</returns>
        /// <exception cref="Exception">Throws exception if unexpected error happens.</exception>
        public static Result Query(string host,int port,Socket socket)
        {
            if(host == null){
                throw new ArgumentNullException("host");
            }
            if(socket == null){
                throw new ArgumentNullException("socket");
            }
            if(port < 1){
                throw new ArgumentException("Port value must be >= 1 !");
            }
            if(socket.ProtocolType != ProtocolType.Udp){
                throw new ArgumentException("Socket must be UDP socket !");
            }

            // Add try/catch to if the host cannot be resolved
            IPEndPoint remoteEndPoint = null;
            try
            {
                remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            } catch(SocketException)
            {
                return new Result(NetType.NotAvailable, null);
            }
            
            socket.ReceiveTimeout = 3000;
            socket.SendTimeout = 3000;

            /*
                In test I, the client sends a STUN Binding Request to a server, without any flags set in the
                CHANGE-REQUEST attribute, and without the RESPONSE-ADDRESS attribute. This causes the server 
                to send the response back to the address and port that the request came from.
            
                In test II, the client sends a Binding Request with both the "change IP" and "change port" flags
                from the CHANGE-REQUEST attribute set.  
              
                In test III, the client sends a Binding Request with only the "change port" flag set.
                          
                                    +--------+
                                    |  Test  |
                                    |   I    |
                                    +--------+
                                         |
                                         |
                                         V
                                        /\              /\
                                     N /  \ Y          /  \ Y             +--------+
                      UDP     <-------/Resp\--------->/ IP \------------->|  Test  |
                      Blocked         \ ?  /          \Same/              |   II   |
                                       \  /            \? /               +--------+
                                        \/              \/                    |
                                                         | N                  |
                                                         |                    V
                                                         V                    /\
                                                     +--------+  Sym.      N /  \
                                                     |  Test  |  UDP    <---/Resp\
                                                     |   II   |  Firewall   \ ?  /
                                                     +--------+              \  /
                                                         |                    \/
                                                         V                     |Y
                              /\                         /\                    |
               Symmetric  N  /  \       +--------+   N  /  \                   V
                  NAT  <--- / IP \<-----|  Test  |<--- /Resp\               Open
                            \Same/      |   I    |     \ ?  /               Internet
                             \? /       +--------+      \  /
                              \/                         \/
                              |                           |Y
                              |                           |
                              |                           V
                              |                           Full
                              |                           Cone
                              V              /\
                          +--------+        /  \ Y
                          |  Test  |------>/Resp\---->Restricted
                          |   III  |       \ ?  /
                          +--------+        \  /
                                             \/
                                              |N
                                              |       Port
                                              +------>Restricted

            */

            // Test I
            MessageC test1 = new MessageC();
            test1.Type = MessageType.BindingRequest;
            MessageC test1response = DoTransaction(test1,socket,remoteEndPoint);
    
            // UDP blocked.
            if(test1response == null){
                return new Result(NetType.UdpBlocked,null);
            }
            else{
                // Test II
                MessageC test2 = new MessageC();
                test2.Type = MessageType.BindingRequest;
                test2.ChangeRequest = new TChangeRequest(true,true);

                // No NAT.
                if(socket.LocalEndPoint.Equals(test1response.MappedAddress)){
                    MessageC test2Response = DoTransaction(test2,socket,remoteEndPoint);
                    // Open Internet.
                    if(test2Response != null){
                        return new Result(NetType.OpenInternet,test1response.MappedAddress);
                    }
                    // Symmetric UDP firewall.
                    else{
                        return new Result(NetType.SymmetricUdpFirewall,test1response.MappedAddress);
                    }
                }
                // NAT
                else{
                    MessageC test2Response = DoTransaction(test2,socket,remoteEndPoint);
                    // Full cone NAT.
                    if(test2Response != null){
                        return new Result(NetType.FullCone,test1response.MappedAddress);
                    }
                    else{
                        /*
                            If no response is received, it performs test I again, but this time, does so to 
                            the address and port from the CHANGED-ADDRESS attribute from the response to test I.
                        */

                        // Test I(II)
                        MessageC test12 = new MessageC();
                        test12.Type = MessageType.BindingRequest;

                        MessageC test12Response = DoTransaction(test12,socket,test1response.ChangedAddress);
                        if(test12Response == null){
                            throw new Exception("STUN Test I(II) dind't get resonse !");
                        }
                        else{
                            // Symmetric NAT
                            if(!test12Response.MappedAddress.Equals(test1response.MappedAddress)){
                                return new Result(NetType.Symmetric,test1response.MappedAddress);
                            }
                            else{
                                // Test III
                                MessageC test3 = new MessageC();
                                test3.Type = MessageType.BindingRequest;
                                test3.ChangeRequest = new TChangeRequest(false,true);

                                MessageC test3Response = DoTransaction(test3,socket,test1response.ChangedAddress);
                                // Restricted
                                if(test3Response != null){
                                    return new Result(NetType.RestrictedCone,test1response.MappedAddress);
                                }
                                // Port restricted
                                else{
                                    return new Result(NetType.PortRestrictedCone,test1response.MappedAddress);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region method GetSharedSecret

        private void GetSharedSecret()
        {
            /*
                *) Open TLS connection to STUN server.
                *) Send Shared Secret request.
            */

            /*
            using(SocketEx socket = new SocketEx()){
                socket.RawSocket.ReceiveTimeout = 5000;
                socket.RawSocket.SendTimeout = 5000;

                socket.Connect(host,port);
                socket.SwitchToSSL_AsClient();                

                // Send Shared Secret request.
                Message sharedSecretRequest = new Message();
                sharedSecretRequest.Type = MessageType.SharedSecretRequest;
                socket.Write(sharedSecretRequest.ToByteData());
                
                // TODO: Parse message

                // We must get  "Shared Secret" or "Shared Secret Error" response.

                byte[] receiveBuffer = new byte[256];
                socket.RawSocket.Receive(receiveBuffer);

                Message sharedSecretRequestResponse = new Message();
                if(sharedSecretRequestResponse.Type == MessageType.SharedSecretResponse){
                }
                // Shared Secret Error or Unknown response, just try again.
                else{
                    // TODO: Unknown response
                }
            }*/
        }

        #endregion

        #region method DoTransaction

        /// <summary>
        /// Does STUN transaction. Returns transaction response or null if transaction failed.
        /// </summary>
        /// <param name="request">STUN message.</param>
        /// <param name="socket">Socket to use for send/receive.</param>
        /// <param name="remoteEndPoint">Remote end point.</param>
        /// <returns>Returns transaction response or null if transaction failed.</returns>
        private static MessageC DoTransaction(MessageC request,Socket socket,IPEndPoint remoteEndPoint)
        {                        
            byte[] requestBytes = request.ToByteData();                              
            DateTime startTime = DateTime.Now;
            // We do it only 2 sec and retransmit with 100 ms.
            while(startTime.AddSeconds(2) > DateTime.Now){
                try{
                    socket.SendTo(requestBytes,remoteEndPoint);

                    // We got response.
                    if(socket.Poll(100,SelectMode.SelectRead)){
                        byte[] receiveBuffer = new byte[512];
                        socket.Receive(receiveBuffer);

                        // Parse message
                        MessageC response = new MessageC();
                        response.Parse(receiveBuffer);

                        // Check that transaction ID matches or not response what we want.
                        if(request.TransactionID.Equals(response.TransactionID)){
                            return response;
                        }
                    }
                }
                catch{
                }
            }

            return null;
        }

        #endregion

    }
}
