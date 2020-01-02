using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Models
{
    // Add profile data for application users by adding propertites to the ApplicationUser class
    /// <summary>
    /// 应用用户（继承自IdentityUser，扩展自有用户属性）
    /// </summary>
    public class ApplicationUser: IdentityUser
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(63)]
        public string NickName { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        [MaxLength(63)]
        public string WXOpenId { get; set; }
        /// <summary>
        /// 微信登陆名
        /// </summary>
        [MaxLength(63)]
        public string WXLoginName { get; set; }
        /// <summary>
        /// 标准化的登录名
        /// </summary>
        [MaxLength(63)]
        public string NormalizedWXLoginName { get; set; }
        /// <summary>
        /// 微信登陆名是否确认
        /// </summary>
        public bool WXLoginNameConfirmed { get; set; }
        #region 版本以及数据库跟踪信息
        /// <summary>
        /// 用户信息版本时间戳unix utc
        /// </summary>
        public long Version { get; set; }
        /// <summary>
        /// 新建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now.ToCstTime();
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now.ToCstTime();
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }
        #endregion
        /// <summary>
        /// 住址
        /// </summary>
        [MaxLength(127)]
        public string Address { get; set; }
    }
}
