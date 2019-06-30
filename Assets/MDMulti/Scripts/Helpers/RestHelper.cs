using Debug = UnityEngine.Debug;

namespace MDMulti
{
    public class RestHelper
    {
        public static void DownloadServerCertificate()
        {
            Debug.Log("B");
            Mono.Main.Inst.StartCoroutine(Rest.Get("server-public", res =>
            {
                Debug.Log("CERT: " + res.ContentType());
                Debug.Log("Cert: " + res.ResponseData());
                StorageHelper.SaveToFile(res.ResponseData(), "server-public.crt");
            }));
        }

        public static void ConnectionTest()
        {
            Mono.Main.Inst.StartCoroutine(Rest.Get("info", res =>
            {
                Debug.Log("CT-X");
                Debug.Log(res.ResponseCode() == 200);
            }));
        }
    }
}