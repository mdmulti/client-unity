// Original project copyright (c) Ivar Lumi 2007
// Licensed under CPOL. See CPOL.html for more details.
// https://www.codeproject.com/Articles/18492/STUN-Client

using System.Net;

namespace MDMulti.STUN.Client
{
    /// <summary>
    /// This class holds STUN_Client.Query method return data.
    /// </summary>
    public class Result
    {
        private NetType m_NetType = STUN.Client.NetType.OpenInternet;
        private IPEndPoint m_pPublicEndPoint = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="netType">Specifies UDP network type.</param>
        /// <param name="publicEndPoint">Public IP end point.</param>
        public Result(NetType netType,IPEndPoint publicEndPoint)
        {            
            m_NetType = netType;
            m_pPublicEndPoint = publicEndPoint;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets UDP network type as an enum.
        /// </summary>
        public NetType NetTypeEnum
        {
            get{ return m_NetType; }
        }

        /// <summary>
        /// Gets a user friendly UDP network type format response.
        /// </summary>
        public NetTypeConverter NetType
        {
            get { return new NetTypeConverter(m_NetType); }
        }

        /// <summary>
        /// Gets public IP end point. This value is null if failed to get network type.
        /// </summary>
        public IPEndPoint PublicEndPoint
        {
            get{ return m_pPublicEndPoint; }
        }

        #endregion

    }
}
