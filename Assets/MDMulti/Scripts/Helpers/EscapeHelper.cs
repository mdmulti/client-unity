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
    }
}