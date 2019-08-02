// Original project copyright (c) Ivar Lumi 2007
// Licensed under CPOL. See CPOL.html for more details.
// https://www.codeproject.com/Articles/18492/STUN-Client

namespace MDMulti.STUN.Message
{
    /// <summary>
    /// This class implements STUN CHANGE-REQUEST attribute. Defined in RFC 3489 11.2.4.
    /// </summary>
    public class TChangeRequest
    {
        private bool m_ChangeIP   = true;
        private bool m_ChangePort = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TChangeRequest()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="changeIP">Specifies if STUN server must send response to different IP than request was received.</param>
        /// <param name="changePort">Specifies if STUN server must send response to different port than request was received.</param>
        public TChangeRequest(bool changeIP,bool changePort)
        {
            m_ChangeIP = changeIP;
            m_ChangePort = changePort;
        }


        #region Properties Implementation

        /// <summary>
        /// Gets or sets if STUN server must send response to different IP than request was received.
        /// </summary>
        public bool ChangeIP
        {
            get{ return m_ChangeIP; }

            set{ m_ChangeIP = value; }
        }

        /// <summary>
        /// Gets or sets if STUN server must send response to different port than request was received.
        /// </summary>
        public bool ChangePort
        {
            get{ return m_ChangePort; }

            set{ m_ChangePort = value; }
        }

        #endregion

    }
}
