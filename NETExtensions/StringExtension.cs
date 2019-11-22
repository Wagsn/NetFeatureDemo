using System;
using System.Collections.Generic;
using System.Text;

namespace NETExtensions
{
    public static class StringExtension
    {
        public static System.IO.MemoryStream ToStream(this string source)
        {
            return new System.IO.MemoryStream(source.ToBytes());
        }

        public static byte[] ToBytes(this string source)
        {
            return Encoding.ASCII.GetBytes(source);
        }
    }
}
