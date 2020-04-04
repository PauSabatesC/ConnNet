using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.Utils
{
    internal static class Conversor
    {
        public static String BytesToString(byte[] data)
        {
            return System.Text.Encoding.Default.GetString(data);
        }
        public static byte[] StringToBytes(string data)
        {
            byte[] dataB = Encoding.ASCII.GetBytes(data);
            return dataB;
        }
    }
}
