using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFeatureDemo.Tests
{
    public abstract class TestBase : ITest
    {
        public TestBase()
        {
            Name = GetType().Name;
        }

        public string Name { get; set; }

        public abstract void Test(string[] args);
    }
}
