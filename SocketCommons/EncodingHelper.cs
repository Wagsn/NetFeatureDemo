using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommons
{
    /// <summary>
    /// 编码助手
    /// </summary>
    public class EncodingHelper
    {
        public static byte[] Encoding(string src)
        {
            return System.Text.Encoding.UTF8.GetBytes(src);
        }

        public static string Decoding(byte[] src)
        {
            return System.Text.Encoding.UTF8.GetString(src);
        }
        public static string Decoding(byte[] src, int index, int count)
        {
            return System.Text.Encoding.UTF8.GetString(src, index, count);
        }
    }
}
