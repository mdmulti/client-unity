using System.Threading.Tasks;
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

        public static async Task<bool> ConnectionTest()
        {
            return (await Rest.GetAsync("info")).ResponseCode() == 200;
        }
    }
}