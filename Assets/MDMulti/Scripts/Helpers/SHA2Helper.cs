using System;
using System.Security.Cryptography;
using System.Text;

namespace MDMulti
{
    public class SHA2Helper
    {
        public static byte[] ComputeHash(byte[] data)
        {
            using (SHA256Managed sha2 = new SHA256Managed())
            {
                return sha2.ComputeHash(data);
            }
        }

        /// <summary>
        /// Compute the hash for the provided string and return as hex.
        /// </summary>
        /// <param name="data">ASCII Encoded string data</param>
        /// <returns>Hexadecimal hash as a string.</returns>
        public static string ComputeHashStr(string data)
        {
            return BitConverter.ToString(ComputeHash(Encoding.ASCII.GetBytes(data))).Replace("-", string.Empty);
        }

    }
}