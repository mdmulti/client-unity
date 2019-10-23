using System.Net;
using System.Threading.Tasks;

namespace MDMulti.Net
{
    public class SecureClient : PlainClient
    {
        private UserFile uf;
        private IPEndPoint ipep;

        public SecureClient(UserFile uf, IPEndPoint ipep) : base(ipep) {
            this.uf = uf;
        }
        
        public new async Task<string> Send(string sdata)
        {
            return uf.DecryptStr2(await base.Send(uf.EncryptStr2(sdata)));
        }
    }
}
