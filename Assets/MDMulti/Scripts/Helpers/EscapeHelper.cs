using System;

namespace MDMulti
{
    public class EscapeHelper
    {
        public static string Escape(string str)
        {
            return Uri.EscapeDataString(str);
        }

        public static string UnEscape(string str)
        {
            return Uri.UnescapeDataString(str);
        }

        public static string B64Escape(string str)
        {
            return str.Replace("/", "_");
        }

        public static string B64UnEscape(string str)
        {
            return str.Replace("_", "/");
        }
    }
}