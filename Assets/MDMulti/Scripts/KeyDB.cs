using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MDMulti
{
    public class PeerDB
    {
        /// <summary>
        /// Get the PeerDB / KeyDB constants object.
        /// See object <seealso cref="Constants.PeerdbFile"/>.
        /// </summary>
        /// <returns>The constants object.</returns>
        private static Constants.PeerdbFile GetConstants()
        {
            return ConstantsHelper.Get().PeerdbFile;
        }

        /// <summary>
        /// Create or load an object.
        /// </summary>
        public static async Task<KeyFile> GetObject()
        {
            if (!await StorageHelper.FileExistsAndIsJson(GetConstants().Name))
            {
                UnityEngine.Debug.Log("KEYDB - Creating new");
                return new KeyFile();
            } else
            {
                UnityEngine.Debug.Log("KEYDB - Loading");
                return await LoadFromFile();
            }
        }

        private static async Task<KeyFile> LoadFromFile()
        {
            string data = Encoding.UTF8.GetString(await StorageHelper.ReadFileByte(GetConstants().Name));
            return JsonConvert.DeserializeObject<KeyFile>(data);
        }

        /// <summary>
        /// Schema / Class file for the Root Key File
        /// </summary>
        public class KeyFile
        {
            // We do it this way so that it is still serialized as JSON.
            public int version { get { return (int)GetConstants().Version; } }
            public List<KeyItem> keys;

            public KeyFile()
            {
                keys = new List<KeyItem>();
            }

            public void SaveToFile()
            {
                StorageHelper.SaveToFileAlternate(JsonConvert.SerializeObject(this), GetConstants().Name);
            }
            
            /// <summary>
            /// Add a new certificate item to the database, even if it already exists.
            /// </summary>
            /// <param name="cert"></param>
            public void AddX509Force(X509Certificate2 cert)
            {
                keys.Add(new KeyItem(cert));
            }

            /// <summary>
            /// Add a new certificate item to the database if it was not already present.
            /// </summary>
            public void AddX509IfNotPresent(X509Certificate2 cert)
            {
                KeyItem certKI = new KeyItem(cert);

                bool exists = keys.Any(item => item.GetAID() == certKI.GetAID() && item.GetPublicKeyString() == certKI.GetPublicKeyString());
                
                if (!exists) AddX509Force(cert);
            }
            
        }

        /// <summary>
        /// Schema / Class file for the Key Item(s)
        /// </summary>
        public class KeyItem
        {
            public KeyItem(X509Certificate2 cert)
            {
                _x509Pub = cert;
            }

            [JsonProperty("apparentID")]
            // This object has been made private so that we can
            // freely rename these in the future without affecting compatability.
            private string _apparentID { get { return GetCert().GetSerialNumberString().ToUpper(); } }


            [JsonProperty("x509Pub")]
            [JsonConverter(typeof(X509Certificate2Converter))]
            // This object has been made private so that we can
            // freely rename these in the future without affecting compatability.
            private X509Certificate2 _x509Pub;
            
            /// <summary>
            /// Get the X509Certificate2 object for this item.
            /// </summary>
            public X509Certificate2 GetCert()
            {
                return _x509Pub;
            }

            /// <summary>
            /// Get the apparent ID for this item.
            /// </summary>
            public string GetAID()
            {
                return _apparentID;
            }

            /// <summary>
            /// Get the public key for this item as a string.
            /// </summary>
            /// <returns>The string representation of the public key.</returns>
            public string GetPublicKeyString()
            {
                return GetCert().GetPublicKeyString();
            }
        }

        /// <summary>
        /// NewtonSoft.JSON converter to serialize an X509Certificate2 to a Base64 string.
        /// Uses <seealso cref="CertHelper.ExportCertificate(X509Certificate2)"/>
        /// </summary>
        public class X509Certificate2Converter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                X509Certificate2 cert = value as X509Certificate2;
                writer.WriteValue(CertHelper.ExportCertificate(cert));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                byte[] certData = Convert.FromBase64String((string)reader.Value);
                return new X509Certificate2(certData);
            }

            // CanRead is not required as it is enabled by default
            // It also takes no arguments

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(X509Certificate2);
            }
        }
    }
}
