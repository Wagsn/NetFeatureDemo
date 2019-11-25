using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommons
{
    /// <summary>
    /// 协议数据
    /// </summary>
    public class ProtocolData
    {
        /// <summary>
        /// 协议头，包含协议版本等各种信息
        /// </summary>
        public List<Attribute> Headers { get; set; }
        /// <summary>
        /// 携带的数据
        /// TODO 封装成字节流的形式访问
        /// </summary>
        public byte[] Content { get; set; }
    }

    public class Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
