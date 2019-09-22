﻿using System;
using System.Text;

namespace MDMulti.Net
{
    public class GeneralCore
    {
        public static byte[] AddInfoData(byte[] data)
        {
            LAN.Discovery.Message m = new LAN.Discovery.Message();
            string h = m.server + "/" + m.escapedApplicationName + "/////";

            byte[] headerb = Encoding.UTF8.GetBytes(h);

            byte[] res = new byte[data.Length + headerb.Length];
            System.Buffer.BlockCopy(headerb, 0, res, 0, headerb.Length);
            System.Buffer.BlockCopy(data, 0, res, headerb.Length, data.Length);

            return res;

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