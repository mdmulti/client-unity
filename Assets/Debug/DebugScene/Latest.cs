using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class Latest : MonoBehaviour
    {
        public async void Latest1()
        {
            Debug.Log("L");

            // KEYDB Testing | AddIfNotPresent

            MDMulti.PeerDB.KeyFile s = await MDMulti.PeerDB.GetObject();

            s.AddX509IfNotPresent(await MDMulti.CertHelper.GetCertificateFromFile("p4.crt"));
            s.SaveToFile();
        }

        public async void Latest2()
        {
            Debug.Log("L2");

            // KEYDB Testing

            var kf = await MDMulti.PeerDB.GetObject();

            Debug.Log(kf.keys[0].GetAID());
            Debug.Log(MDMulti.CertHelper.IsUserCertificate(kf.keys[0].GetCert()));

        }
    }
}