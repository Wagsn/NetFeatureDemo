using System;
using System.Collections.Generic;
using System.Text;

namespace SocketServer
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int UserPort { get; set; }

        /// <summary>
        /// 接入点
        /// 如：127.0.0.1:1234
        /// </summary>
        public string EndPoint { get; set; }

    }
}
