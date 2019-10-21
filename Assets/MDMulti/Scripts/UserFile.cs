using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// UserFile constructor - loads from a .mdmc file.
        /// </summary>
        /// <param name="clientID">the client ID or filename to load (exluding ext)</param>
        public UserFile(string clientID)
        {
            string filename = clientID + ".mdmc";
            
            if (StorageHelper.FileExists(filename))
            {
                // Load the file
                byte[] raw = StorageHelper.ReadFileByteSync(filename);

                // Convert it from JSON into an object
                JSONSchema data = JsonConvert.DeserializeObject<JSONSchema>(Encoding.UTF8.GetString(raw));

                // Decode the Keypairs
                byte[] keypairsB = Convert.FromBase64String(data.keypairs);

                // Create the certificate
                X509Certificate2 cert = new X509Certificate2(keypairsB, "", X509KeyStorageFlags.PersistKeySet);
                
                // Run the usual init for certificates.
                Setup(cert);
            }
        }

        public UserFile(X509Certificate2 cert)
        {
            Setup(cert);
        }

        private void Setup(X509Certificate2 cert)
        {
            if (CertHelper.IsUserCertificate(cert))
            {
                Cert = cert;
                ID = cert.GetSerialNumberString();
                ServerID = CertHelper.GetCustomOIDValue(cert.Extensions[CertHelper.oid_base + ".3"]);

                // As only specifying a certificate does not give us a display name, we will just set it to unknown.
                DisplayName = "Unknown";
            }
            else
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

            try
            {
                json.keypairs = CertHelper.ExportKeyPairs(Cert);
            } catch (InvalidCertificateException)
            {
                UnityEngine.Debug.LogWarning("JSON PRIV NULL");
                json.keypairs = null;
            }

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
            public readonly int version = 3;

            public string id;
            public string serverId;
            public string displayName;
            public string keypairs;

            public JSONSchema()
            {

            }
        }
    }
}