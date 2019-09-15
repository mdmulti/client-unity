using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MDMulti
{
    public class CertHelper
    {
        private static readonly string oid = "1.3.6.1.4.1.37476.9000.83.1";
        private static readonly string oid_client_proto_version = "1.3.6.1.4.1.37476.9000.83.1.1";
        private static readonly string oid_type = "1.3.6.1.4.1.37476.9000.83.1.2";

        /// <summary>
        /// Takes in a user certificate and verifies that it was signed by the active server certificate.
        /// </summary>
        /// <param name="user">The user certificate.</param>
        /// <returns>Boolean of weather the user has been verified</returns>
        public static async Task<bool> VerifyUserCert(X509Certificate2 user)
        {
            X509Certificate2 server = await GetServerCert();

            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            chain.ChainPolicy.ExtraStore.Add(server);
            chain.Build(user);
            if (chain.ChainStatus.Length == 1 &&
                chain.ChainStatus.First().Status == X509ChainStatusFlags.UntrustedRoot)
            {
                // chain is valid, thus cert signed by root certificate 
                // and we expect that root is untrusted which the status flag tells us
                return true;
            }
            else
            {
                // not valid for one or more reasons
                return false;
            }
        }

        /// <summary>
        /// Gets the server certificate.
        /// </summary>
        /// <returns>An X509Certificate2 with the certificate.</returns>
        public static async Task<X509Certificate2> GetServerCert()
        {
            string filename = GetFirstServerCertFileName();

            // Download the certificate if it is not already present
            if (!StorageHelper.FileExists(filename))
            {
                RestHelper.DownloadServerCertificate();
            }

            X509Certificate2 cert = new X509Certificate2();
            cert.Import(await StorageHelper.ReadFileByte(filename));
            return cert;
        }

        /// <summary>
        /// Get the filename of the first found server certificate.
        /// </summary>
        /// <returns>String filename in the game's AppData directory.</returns>
        public static string GetFirstServerCertFileName()
        {
            DirectoryInfo di = new DirectoryInfo(StorageHelper.GetFullPath(""));
            FileInfo[] files = di.GetFiles("*.server-public.crt").OrderBy(f => f.LastWriteTime).ToArray();
            if (files.Length > 0)
            {
                return files[0].Name;
            }
            else
            {
                UnityEngine.Debug.LogError("Could not get ServerCertFileName!");
                return null;
            }
        }


        /// <summary>
        /// Get the filename of the first found user certificate.
        /// </summary>
        /// <returns>String filename in the game's AppData directory.</returns>
        public static string GetFirstUserCertFileName()
        {
            DirectoryInfo di = new DirectoryInfo(StorageHelper.GetFullPath(""));
            FileInfo[] files = di.GetFiles("*.user.crt").OrderBy(f => f.LastWriteTime).ToArray();
            if (files.Length > 0)
            {
                return files[0].Name;
            }
            else
            {
                UnityEngine.Debug.LogError("Could not get UserCertFileName!");
                return null;
            }
        }

        /// <summary>
        /// Load a certificate from a relative filename in the game's data directory.
        /// </summary>
        /// <param name="filename">The certificate filename to load</param>
        /// <returns>The loaded certificate</returns>
        public static async Task<X509Certificate2> GetCertificateFromFile(string filename)
        {
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(await StorageHelper.ReadFileByte(filename));
            return cert;
        }

        /// <summary>
        /// Tests to see if the certificate provided has the required data for an MDMulti User certificate.
        /// </summary>
        /// <param name="cert"> The certificate to test</param>
        /// <returns></returns>
        public static bool IsUserCertificate(X509Certificate2 cert)
        {
            UnityEngine.Debug.Log("LEN: " + cert.Extensions.Count);
            UnityEngine.Debug.Log("COIDV: " + GetCustomOIDValue(cert.Extensions[5]));
            UnityEngine.Debug.Log("COIDV VIA TXT: " + GetCustomOIDValue(cert.Extensions[oid_client_proto_version]));
            UnityEngine.Debug.Log("ALLOID: " + string.Join(", ", GetAllCustomOIDValues(cert.Extensions)));
            

            return (cert.Extensions[oid] != null && GetCustomOIDValue(cert.Extensions[oid_type]) == "1");
        }

        /// <summary>
        /// Converts a hex string to an ASCII string.
        /// </summary>
        /// <param name="hexString">The hex string to convert</param>
        /// <returns></returns>
        public static string ConvertHex(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }

        /// <summary>
        /// Gets the data from a custom certificate extension. [oid]
        /// </summary>
        /// <param name="extension">The extension to use.</param>
        /// <returns></returns>
        public static string GetCustomOIDValue(X509Extension extension)
        {
            ASN1.ASN1Element element = new ASN1.ASN1Element(extension.RawData, 0);
            return ConvertHex(BitConverter.ToString(element.Value).Replace("-", "").Replace("00", ""));
        }

        /// <summary>
        /// Similar to <see cref="GetCustomOIDValue(X509Extension)"/>, but returrns multiple values.
        /// Returns all OID values in the extension collection.
        /// </summary>
        /// <param name="extensionCollection">The extension collection to use.</param>
        /// <returns></returns>
        public static List<string> GetAllCustomOIDValues(X509ExtensionCollection extensionCollection)
        {
            List<string> arr = new List<string>();

            foreach (var ext in extensionCollection)
            {
                arr.Add(GetCustomOIDValue(ext));
            }

            return arr;
        }
    }
}