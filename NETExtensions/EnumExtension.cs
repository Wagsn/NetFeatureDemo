using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NETExtensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 返回枚举的描述
        /// 如果有[Description(val)]返回val，没有返回enum.ToString()
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum val)
        {
            var type = val.GetType();

            var memberInfo = type.GetMember(val.ToString());

            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes == null || attributes.Length != 1)
            {
                //如果没有定义描述，就把当前枚举值的对应名称返回
                return val.ToString();
            }

            return (attributes.Single() as DescriptionAttribute).Description;
        }
    }
}
