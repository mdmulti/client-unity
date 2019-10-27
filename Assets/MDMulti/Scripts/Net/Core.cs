using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MDMulti.Net
{
    public class Core
    {
        public static string GetInfoData()
        {
            LAN.Discovery.Message m = new LAN.Discovery.Message();
            string header = "MDMNET_" + m.escapedApplicationName + "_";
            return header;
        }

        public static byte[] AddInfoData(byte[] data)
        {
            string h = GetInfoData();

            byte[] headerb = Encoding.UTF8.GetBytes(h);

            byte[] res = new byte[data.Length + headerb.Length];
            Buffer.BlockCopy(headerb, 0, res, 0, headerb.Length);
            Buffer.BlockCopy(data, 0, res, headerb.Length, data.Length);

            return res;

        }

        public static byte[] GenerateAndAddHash(byte[] data)
        {
            string hashStr = SHA2Helper.ComputeHashStr(Encoding.UTF8.GetString(data));
            byte[] hash = Encoding.UTF8.GetBytes(hashStr);

            // We now have, in the hash byte array, the hex output of the hash function

            byte[] res = new byte[data.Length + hash.Length];
            Buffer.BlockCopy(data, 0, res, 0, data.Length);
            Buffer.BlockCopy(hash, 0, res, data.Length, hash.Length);

            return res;
        }

        public static string GetHash(byte[] data)
        {
            byte[] res = new byte[64];
            Buffer.BlockCopy(data, (data.Length - 64), res, 0, 64);
            return Encoding.UTF8.GetString(res);
        }

        public static Tuple<byte[], string> SplitHashAndMessage(byte[] data)
        {
            byte[] data_res = new byte[data.Length - 64];
            Buffer.BlockCopy(data, 0, data_res, 0, data.Length - 64);
            return new Tuple<byte[], string>(data_res, GetHash(data));
        }

        public string ParseResponse(string data)
        {
            if (data.StartsWith(GetInfoData(), StringComparison.Ordinal))
            {
                return StringFromBase64(data.Split('_')[2]);
            } else
            {
                return "MDMNET_ERR_INVALID_RESPONSE";
            }
        }

        public string StringToBase64(string data)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(data));
        }

        public string StringFromBase64(string data)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(data));
        }
    }

    public class DataConnectionRefused : Exception
    {
        public DataConnectionRefused() { }

        public DataConnectionRefused(string message) : base(message) { }

        public DataConnectionRefused(string message, Exception inner) : base(message, inner) { }
    }

    public enum DataTypes
    {
        ReliableTCP = 0,
        UnreliableUDP = 1,
    }
}