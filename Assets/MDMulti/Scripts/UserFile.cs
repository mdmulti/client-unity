using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MDMulti
{
    public class UserFile
    {
        /// <summary>
        /// The algorithm used to encrypt and decrypt data.
        /// </summary>
        private readonly static RSAEncryptionPadding RSAEncryptionType = RSAEncryptionPadding.OaepSHA1;

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

        public byte[] EncryptBytes(byte[] data)
        {
            UnityEngine.Debug.Log("ENC_BEF: " + Encoding.ASCII.GetString(data));
            RSACryptoServiceProvider csp = Cert.PublicKey.Key as RSACryptoServiceProvider;
            byte[] enc = csp.Encrypt(data, false);
            UnityEngine.Debug.Log("ENC_AFT: " + Encoding.ASCII.GetString(enc));
            return enc;
        }

        public byte[] DecryptBytes(byte[] data)
        {
            UnityEngine.Debug.Log("DEC_BEF: " + Encoding.ASCII.GetString(data));
            RSACryptoServiceProvider csp = Cert.PrivateKey as RSACryptoServiceProvider;
            byte[] enc = csp.Encrypt(data, false);
            UnityEngine.Debug.Log("DEC_AFT: " + Encoding.ASCII.GetString(enc));
            return enc;
        }

        public string EncryptB64(string data)
        {
            byte[] b_enc_in_not = EncryptBytes(Encoding.ASCII.GetBytes(data));
            UnityEngine.Debug.Log("ENC_64--ENCODED NOT 64: " + Encoding.ASCII.GetString(b_enc_in_not));

            string enc64 = Convert.ToBase64String(b_enc_in_not);
            UnityEngine.Debug.Log("ENC_64--ENCODED YES 64: " + enc64);

            return enc64;
            //return Encoding.ASCII.GetString(EncryptBytes(Convert.ToBase64String(Encoding.ASCII.GetBytes(data))));
        }

        public string DecryptB64(string data)
        {
            byte[] data_not64 = Convert.FromBase64String(data);

            UnityEngine.Debug.Log("DEC_64--RAW YES 64: " + data);
            UnityEngine.Debug.Log("DEC_64--RAW NOT 64: " + Encoding.ASCII.GetString(data_not64));

            byte[] dec_not64 = DecryptBytes(data_not64);

            UnityEngine.Debug.Log("DEC_64--RES: " + Encoding.ASCII.GetString(dec_not64));

            return Encoding.ASCII.GetString(dec_not64);
            //return Encoding.ASCII.GetString(DecryptBytes(Convert.FromBase64String(data)));
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