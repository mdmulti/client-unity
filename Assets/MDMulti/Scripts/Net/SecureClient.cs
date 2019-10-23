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
            /*UnityEngine.Debug.LogError("SC_SEND_INPUT: " + sdata);

            string enc = uf.EncryptStr2(sdata);
            UnityEngine.Debug.LogError("SC_SEND_INPUT_ENC: " + enc);

            string res_enc = await base.Send(enc);
            UnityEngine.Debug.LogError("SC_SEND_RES_ENC: " + res_enc);

            string res = uf.DecryptStr2(res_enc);
            UnityEngine.Debug.LogError("SC_SEND_RES: " + res);

            return res;*/
            return uf.DecryptStr2(await base.Send(uf.EncryptStr2(sdata)));
        }
    }
}
