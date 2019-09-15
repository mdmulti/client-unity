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
                ServerID = CertHelper.GetCustomOIDValue(cert.Extensions["1.3.6.1.4.1.37476.9000.83.1.3"]);

                // As only specifying a certificate does not give us a display name, we will just set it to unknown.
                DisplayName = "Unknown";
            } else
            {
                throw new InvalidCertificateException("The User Certificate check failed.");
            }
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
    }
}