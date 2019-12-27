using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCoreDemo.Models
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]// FromSql
        public int Id { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }
    }
}
