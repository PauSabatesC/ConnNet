using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SockNet.Utils
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

        public static IPAddress StringToIPAddress(string ip)
        {
            IPAddress iPaddress;
            if (!string.IsNullOrWhiteSpace(ip) && IPAddress.TryParse(ip, out iPaddress)) return IPAddress.Parse(ip);
            else throw new ArgumentException("The IP specified is not valid.", ip);
        }
    }
}
