using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;

namespace MDMulti
{
    public class UserFile
    {
        public X509Certificate2 Cert { get; private set; }

        public string DisplayName { get; private set; }

        public string ServerID { get; private set; }

        public string ID { get; private set; }

        public UserFile()
        {

        }

        public UserFile(X509Certificate2 cert)
        {
            if (CertHelper.IsUserCertificate(cert))
            {
                Cert = cert;
                ID = cert.GetSerialNumberString();
                ServerID = CertHelper.GetCustomOIDValue(cert.Extensions[CertHelper.oid_base + ".3"]);

                // As only specifying a certificate does not give us a display name, we will just set it to unknown.
                DisplayName = "Unknown";
            } else
            {
                throw new InvalidCertificateException("The User Certificate check failed.");
            }
        }

        public void Save()
        {
            JSONSchema json = new JSONSchema();
            json.id = ID;
            json.serverId = ServerID;
            json.displayName = DisplayName;
            json.pubkey = CertHelper.ExportCertificate(Cert);
            //UnityEngine.Debug.Log(json.pubkey);
            //UnityEngine.Debug.Log(JsonConvert.SerializeObject(json));
            StorageHelper.SaveToFileAlternate(JsonConvert.SerializeObject(json), ID + ".mdmc");
        }

        public class InvalidCertificateException : Exception
        {
            public InvalidCertificateException()
            {

            }

            public InvalidCertificateException(string message)
                : base(message)
            {
            }

            public InvalidCertificateException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        public class JSONSchema
        {
            public readonly int version = 1;

            public string id;
            public string serverId;
            public string displayName;
            public string pubkey;

            public JSONSchema()
            {

            }
        }
    }
}