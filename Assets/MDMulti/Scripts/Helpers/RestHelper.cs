﻿using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace MDMulti
{
    public class RestHelper
    {
        public static async void DownloadServerCertificate()
        {
            Debug.Log("B");

            Rest.RequestResponse res = await Rest.GetAsync("server-public");

            if (res.ResponseCode() != 200)
            {
                throw new Rest.ServerNotAvailableException("The server returned HTTP Code " + res.ResponseCode());
            }

            Debug.Log("CERT: " + res.ContentType());
            Debug.Log("Cert: " + res.ResponseData());

            StorageHelper.SaveToFile(res.ResponseData(), res.ServerCertSerial() + ".server-public.crt");
        }

        public static async Task<bool> ConnectionTest()
        {
            return (await Rest.GetAsync("info")).ResponseCode() == 200;
        }
    }
}