using MDMulti;
using UnityEngine;

namespace MDMulti_DEBUG.DebugScene
{
    public class Rest : MonoBehaviour
    {
        public async void CreateUser()
        {
            (await User.CreateNewProfile()).Save();
        }

        public void GetServerCert()
        {
            RestHelper.DownloadServerCertificate();
        }
    }
}