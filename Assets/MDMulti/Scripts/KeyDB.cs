using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace MDMulti.Net
{
    public class KeyDB
    {
        public static KeyFile createNewFile()
        {
            return new KeyFile();
        }

        /// <summary>
        /// Schema / Class file for the Root Key File
        /// </summary>
        public class KeyFile
        {
            // We do it this way so that it is still serialized as JSON.
            public int version { get { return 1; } }
            public List<KeyItem> keys;

            public KeyFile()
            {
                keys = new List<KeyItem>();
            }

            public void SaveToFile()
            {
                StorageHelper.SaveToFileAlternate(JsonConvert.SerializeObject(this), "peerdb.mdm.json");
            }
            
        }

        /// <summary>
        /// Schema / Class file for the Key Item(s)
        /// </summary>
        public class KeyItem
        {
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
                throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            }

            public override bool CanRead
            {
                get { return false; }
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(X509Certificate2);
            }
        }
    }
}
