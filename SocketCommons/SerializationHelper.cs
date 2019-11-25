using Newtonsoft.Json;
using System;

namespace SocketCommons
{
    /// <summary>
    /// 序列化与编码助手
    /// </summary>
    public class SerializationHelper
    {
        /// <summary>
        /// 序列化&编码&加密
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(object obj)
        {
            return EncodingHelper.Encoding(JsonConvert.SerializeObject(obj));
        }

        public static T Deserialize<T>(byte[] src)
        {
            return JsonConvert.DeserializeObject<T>(EncodingHelper.Decoding(src));
        }

        /// <summary>
        /// 解密&解码&反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] src, int index, int count)
        {
            return JsonConvert.DeserializeObject<T>(EncodingHelper.Decoding(src, index, count));
        }
    }
}
