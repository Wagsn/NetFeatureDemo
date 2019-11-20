using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFeatureDemo.Tests
{
    /// <summary>
    /// 测试接口
    /// </summary>
    public interface ITest
    {
        string Name { get; set; }

        void Test(string[] args);
    }
}
