﻿using System;
using System.Text;

namespace MDMulti.Net
{
    public class Core
    {
        public static string GetInfoData()
        {
            LAN.Discovery.Message m = new LAN.Discovery.Message();
            string header = "MDMNET/" + m.escapedApplicationName + "/";
            return header;
        }

        public static byte[] AddInfoData(byte[] data)
        {
            string h = GetInfoData();

            byte[] headerb = Encoding.UTF8.GetBytes(h);

            byte[] res = new byte[data.Length + headerb.Length];
            System.Buffer.BlockCopy(headerb, 0, res, 0, headerb.Length);
            System.Buffer.BlockCopy(data, 0, res, headerb.Length, data.Length);

            return res;

        }

        public string ParseResponse(string data)
        {
            if (data.StartsWith(GetInfoData(), StringComparison.Ordinal))
            {
                return StringFromBase64(data.Split('/')[2]);
            } else
            {
                return "MDMNET_ERR_INVALID_RESPONSE";
            }
        }

        public string StringToBase64(string data)
        {
            return EscapeHelper.B64Escape(Convert.ToBase64String(Encoding.ASCII.GetBytes(data)));
        }

        public string StringFromBase64(string data)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(EscapeHelper.B64UnEscape(data)));
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