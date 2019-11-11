using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MDMulti
{
    public class KeyDB
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

        // TODO: REMOVE
        public static KeyFile CreateNewFile()
        {
            return new KeyFile();
        }

        public static async Task<KeyFile> LoadFromFile()
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
            /// Add a new certificate item to the database.
            /// </summary>
            /// <param name="cert"></param>
            public void AddX509(X509Certificate2 cert)
            {
                keys.Add(new KeyItem(cert));
            }
            
        }

        /// <summary>
        /// Schema / Class file for the Key Item(s)
        /// </summary>
        public class KeyItem
        {
            public KeyItem(X509Certificate2 cert)
            {
                x509Pub = cert;
            }

            public string actualID { get { return x509Pub.GetSerialNumberString(); } }

            [JsonConverter(typeof(X509Certificate2Converter))]
            public X509Certificate2 x509Pub;

            public X509Certificate2 getCert()
            {
                return new X509Certificate2(x509Pub);
            }
        }

        /// <summary>
        /// NewtonSoft JSON converter to serialize an X509Certificate2 to a Base64 string.
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
            // It is also takes no arguments

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(X509Certificate2);
            }
        }
    }
}
