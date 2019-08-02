namespace MDMulti.STUN.Client
{
    /// <summary>
    /// Converts the NetType Enum to something easier to use.
    /// </summary>
    public class NetTypeConverter
    {

        public readonly string Message;
        public readonly string ExtendedMessage;
        public int NATLevel;

        public NetTypeConverter(NetType nte)
        {
            switch (nte)
            {
                case NetType.UdpBlocked:
                    Message = "Strict";
                    ExtendedMessage = "Strict (UDP Blocked)";
                    NATLevel = 0;
                    break;

                case NetType.OpenInternet:
                    Message = "Open";
                    ExtendedMessage = this.Message;
                    NATLevel = 2;
                    break;

                case NetType.SymmetricUdpFirewall:
                    Message = "Strict";
                    ExtendedMessage = "Strict (Symmetric UDP)";
                    NATLevel = 0;
                    break;

                case NetType.FullCone:
                    Message = "Open";
                    ExtendedMessage = "Open (Full Cone)";
                    NATLevel = 2;
                    break;

                case NetType.RestrictedCone:
                    Message = "Moderate";
                    ExtendedMessage = "Moderate (Restricted Cone)";
                    NATLevel = 1;
                    break;

                case NetType.PortRestrictedCone:
                    Message = "Moderate";
                    ExtendedMessage = "Moderate (Port Restricted)";
                    NATLevel = 1;
                    break;

                case NetType.Symmetric:
                    Message = "Strict";
                    ExtendedMessage = "Strict (Symmetric)";
                    NATLevel = 0;
                    break;

                case NetType.NotAvailable:
                    Message = "Not Available";
                    ExtendedMessage = "Not Available (Check your network?)";
                    NATLevel = 0;
                    break;

                default:
                    Message = "Unknown";
                    ExtendedMessage = Message;
                    NATLevel = 0;
                    break;
            }


        }
    }

}