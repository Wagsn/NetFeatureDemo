 using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommons
{
    /// <summary>
    /// 协议助手
    /// </summary>
    public class ProtocolDataHelper
    {
        /// <summary>
        /// 包装 UTF8编码
        /// 将正文包装成ProtocolData再序列化和编码成Byte
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] Packing(string content)
        {
            return SerializationHelper.Serialize(new ProtocolData
            {
                Content = EncodingHelper.Encoding(content)
            });
        }

        /// <summary>
        /// 拆包
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ProtocolData Unpacking(byte[] src, int index, int count)
        {
            return SerializationHelper.Deserialize<ProtocolData>(src, index, count);
        }
    }
}
