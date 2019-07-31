using System.Collections.Generic;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

namespace MDMulti
{
    public class User
    {
        public static async void Create()
        {
            Rest.RequestResponse res = await Rest.GetAsync("users/create");
            Debug.Log("RES TYPE: " + res.Type());
            Debug.Log("RES CODE: " + res.ResponseCode());
            Debug.Log("RES PROTO: " + res.ProtocolVersion());
            Debug.Log("RES DATA: " + res.ResponseData());

            if (res.ResponseCode() == 201)
            {
                string certSerial;
                res.Headers().TryGetValue("X-MDM-CreatedCertSerial", out certSerial);

                Debug.Log("CERT SERIAL: " + certSerial);

                StorageHelper.SaveToFile(res.ResponseData(), certSerial + ".user.crt");
            }
        }
    }
}