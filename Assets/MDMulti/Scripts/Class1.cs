using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MDMulti
{
    public class AES
    {
        private static string TupleToString(Tuple<byte[], byte[]> t)
        {
            return Convert.ToBase64String(t.Item1) + "<MDMSEP>" + Convert.ToBase64String(t.Item2);
        }

        private static Tuple<byte[], byte[]> StringToTuple(string s)
        {
            string[] arr = s.Split(new string[] { "<MDMSEP>" }, StringSplitOptions.None);
            return new Tuple<byte[], byte[]>(Convert.FromBase64String(arr[0]), Convert.FromBase64String(arr[1]));
        }

        // --------------------

        public static string EncToStr(string data)
        {
            return TupleToString(Enc(data));
        }

        public static string DecToStr(string data)
        {
            return Dec(StringToTuple(data));
        }

        private static Tuple<byte[], byte[]> Enc(string data)
        {
            byte[] enc;
            byte[] IV;
            byte[] key;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();

                IV = aesAlg.IV;
                UnityEngine.Debug.Log("IVLEN: " + IV.Length);
                key = aesAlg.Key;
                UnityEngine.Debug.Log("KEY_HEX: " + BitConverter.ToString(key).Replace("-", string.Empty));
                UnityEngine.Debug.Log("IV_HEX: " + BitConverter.ToString(IV).Replace("-", string.Empty));

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (var msEnc = new MemoryStream())
                {
                    using (var csEnc = new CryptoStream(msEnc, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEnc = new StreamWriter(csEnc))
                        {
                            // Write all data to the stream
                            swEnc.Write(data);
                            swEnc.Flush();
                        }
                        enc = msEnc.ToArray();
                        UnityEngine.Debug.Log("DATA_HEX: " + BitConverter.ToString(enc).Replace("-", string.Empty));
                    }
                }
            }

            // combined IV and data
            var combinedIVandCt = new byte[IV.Length + enc.Length];
            Array.Copy(IV, 0, combinedIVandCt, 0, IV.Length);
            Array.Copy(enc, 0, combinedIVandCt, IV.Length, enc.Length);

            return new Tuple<byte[], byte[]>(combinedIVandCt, key);

        }

        private static string Dec(Tuple<byte[], byte[]> d)
        {
            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create the AES obj with the specified key and IV
            //using (Aes aesAlg = Aes.Create())
            //{
            Aes aesAlg = Aes.Create();
                aesAlg.Key = d.Item2;

                byte[] IV = new byte[16];
                byte[] cipherText = new byte[d.Item1.Length - IV.Length];

                Array.Copy(d.Item1, IV, IV.Length);
                Array.Copy(d.Item1, IV.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = IV;

                aesAlg.Mode = CipherMode.CBC;


                UnityEngine.Debug.Log("DEC_KEY_HEX: " + BitConverter.ToString(aesAlg.Key).Replace("-", string.Empty));
                UnityEngine.Debug.Log("DEC_IV_HEX: " + BitConverter.ToString(aesAlg.IV).Replace("-", string.Empty));
                UnityEngine.Debug.Log("DEC_DATA_HEX: " + BitConverter.ToString(cipherText).Replace("-", string.Empty));

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                /*using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }*/
                MemoryStream msDecrypt = new MemoryStream(cipherText);
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                StreamReader srDecrypt = new StreamReader(csDecrypt);

                plaintext = srDecrypt.ReadToEnd();

            //}
            return plaintext;
        }
    }
}
