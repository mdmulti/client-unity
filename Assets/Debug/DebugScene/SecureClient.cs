using MDMulti;
using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class SecureClient : MonoBehaviour
    {
        public void StartServer()
        {
            MDMulti.Net.SecureServer ss = new MDMulti.Net.SecureServer(new UserFile("792F1D97A2740D45867B"), 59655);
            ss.StartListening(serveronrecv);
        }

        public string serveronrecv(string s)
        {
            return "MEMES";
        }

        public async void StartClient()
        {
            MDMulti.Net.SecureClient sc = new MDMulti.Net.SecureClient(new UserFile("792F1D97A2740D45867B"), new System.Net.IPEndPoint(MDMulti.IPHelper.StringToAddressObject("127.0.0.1"), 59655));
            Debug.LogWarning("Final: " + await sc.Send("TEST"));
        }
    }
}