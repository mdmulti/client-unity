using System;

namespace MDMulti.Net
{
    public class SecureServer : PlainServer
    {
        private UserFile uf;

        private Func<string, string> currentOnRecv;

        public SecureServer(UserFile uf, int port) : base(port)
        {
            this.uf = uf;
        }

        public new async void StartListening(Func<string, string> onRecv)
        {
            if (isListening) return;
            isListening = true;
            currentOnRecv = onRecv;
            await ListenUDP(cts.Token, on);
        }

        private string on(string input)
        {
            //return uf.EncryptString(currentOnRecv(uf.DecryptString(input)));

            /*UnityEngine.Debug.LogError("SS_ON_IN: " + input);

            string decIN = uf.DecryptStr2(input);
            UnityEngine.Debug.LogError("SS_ON_DECIN: " + decIN);

            string funcRes = currentOnRecv(decIN);
            UnityEngine.Debug.LogError("SS_ON_FUNCRES: " + funcRes);

            string output = uf.EncryptStr2(funcRes);
            UnityEngine.Debug.LogError("SS_ON_OUT: " + output);

            return output;*/
            return currentOnRecv(input);
        }

        public new void StopListening()
        {
            base.StopListening();
            currentOnRecv = null;
        }
    }
}
