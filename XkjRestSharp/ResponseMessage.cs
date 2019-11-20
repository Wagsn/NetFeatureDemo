using System;
using System.Collections.Generic;
using System.Text;

namespace XkjRestSharp
{
    /// <summary>
    /// 响应体
    /// </summary>
    public class ResponseMessage
    {
        //public bool Result { get => Code == "0"; }

        /// <summary>
        /// 响应码
        /// (ResponseMessage)null)?.Code != "0" == True
        /// (ResponseMessage)null)?.Code == "0" == False
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        public ResponseMessage()
        {
            Code = "0";
        }

        public bool IsSuccess() => Code == "0";
    }

    /// <summary>
    /// Token响应体
    /// </summary>
    public class TokenResponseMessage : ResponseMessage
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// Token类型
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// Token刷新时间
        /// </summary>
        public long expires_in { get; set; }

        /// <summary>
        /// 错误（如："InvalidGrant"）
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// 错误描述（如："账号名或者密码错误"）
        /// </summary>
        public string error_description { get; set; }

        ///// <summary>
        ///// 最后一次接收时间
        ///// </summary>
        //public DateTime LastGetTime { get; set; } = DateTime.Now;
    }

    public class ResponseMessage<TEx> : ResponseMessage
    {
        public TEx Extension { get; set; }
    }

    public class PagingResponseMessage<Tentity> : ResponseMessage<List<Tentity>>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public long TotalCount { get; set; }
    }
}
