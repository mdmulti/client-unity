using MDMulti.Crypto.PEM;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace MDMulti
{
    class PEMCertHelper
    {
        public static X509Certificate2 GetCertificateFromDualPEM(string data)
        {
            byte[] certBuffer = Helpers.GetBytesFromPEM(data, PemStringType.Certificate);
            byte[] keyBuffer = Helpers.GetBytesFromPEM(data, PemStringType.RsaPrivateKey);

            X509Certificate2 certificate = new X509Certificate2(certBuffer, "", X509KeyStorageFlags.Exportable);

            RSACryptoServiceProvider prov = PEM.DecodeRsaPrivateKey(keyBuffer);
            certificate.PrivateKey = prov;

            return certificate;
        }
    }
}
