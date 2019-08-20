using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MDMulti
{
    public class CertHelper
    {
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
            } else
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
    }
}