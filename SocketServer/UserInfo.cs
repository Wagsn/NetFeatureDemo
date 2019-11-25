using System;
using System.Collections.Generic;
using System.Text;

namespace SocketCommons
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
        public string EndPoint 
        { 
            get 
            {
                return $"{UserIp}:{UserPort}";
            } 
            set 
            {
                var args = value.Split(':');
                UserIp = args[0];
                UserPort = int.Parse(args[1]);
            }
        }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 窗口ID，每次打开窗口就会生成
        /// </summary>
        public string WindowId { get; set; }
    }
}
