using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using UnityEngine.Networking;

using Debug = UnityEngine.Debug;

namespace MDMulti
{
    public class User
    {
        public static async Task<UserFile> CreateNewProfile()
        {
            Rest.RequestResponse res = await Rest.GetAsync("users/create");
            Debug.Log("RES TYPE: " + res.Type());
            Debug.Log("RES CODE: " + res.ResponseCode());
            Debug.Log("RES PROTO: " + res.ProtocolVersion());
            Debug.Log("RES DATA: " + res.ResponseData());

            // TEMP REMOVE
            StorageHelper.SaveToFile(res.ResponseData(), "test.crt");

            if (res.ResponseCode() == 201)
            {
                // Create the certificate and private key c# objects
                X509Certificate2 c = PEMCertHelper.GetCertificateFromDualPEM(res.ResponseData());

                // Use that to create the UserFile object
                return new UserFile(c);
            } else
            {
                UnityEngine.Debug.LogError("USER CREATENEW PROFILE RES ERROR");
                return null;
            }
        }
    }
}