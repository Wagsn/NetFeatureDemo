using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.ViewModels
{
    /// <summary>
    /// 注册视图模型
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "电子邮箱不能为空")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "确认密码不能为空")]
        public string ConfirmedPassword { get; set; }
    }
}
