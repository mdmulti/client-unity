using System.Net;

namespace MDMulti.Net
{
    public class SecureClient : PlainClient
    {
        private UserFile uf;

        public SecureClient(UserFile uf, IPEndPoint ipep) : base(ipep) {
            this.uf = uf;
        }
        
        public new string Send(string sdata)
        {
            return base.Send(uf.EncryptString(sdata));
        }
    }
}
