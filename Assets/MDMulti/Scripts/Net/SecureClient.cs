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
            //return uf.DecryptString(base.Send(uf.EncryptString(sdata)));

            UnityEngine.Debug.LogError("SC_SEND_INPUT: " + sdata);

            string enc = uf.EncryptB64(sdata);
            UnityEngine.Debug.LogError("SC_SEND_INPUT_ENC: " + enc);

            string res_enc = base.Send(enc);
            UnityEngine.Debug.LogError("SC_SEND_RES_ENC: " + res_enc);

            string res = uf.DecryptB64(res_enc);
            UnityEngine.Debug.LogError("SC_SEND_RES: " + res);

            return res;
        }
    }
}
