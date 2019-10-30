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
            return uf.EncryptStr2(currentOnRecv(uf.DecryptStr2(input)));
        }

        public new void StopListening()
        {
            base.StopListening();
            currentOnRecv = null;
        }
    }
}
