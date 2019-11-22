using System;
using System.Collections.Generic;
using System.Text;

namespace NetFeatureDemo
{
    /// <summary>
    /// 程序
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
#if CORE20
            HttpWebDemo.Main(args);
#endif
        }

        static void Test(string[] args)
        {
            List<Tests.ITest> tests = new List<Tests.ITest>();
#if CORE20
            tests.Add(new Tests.HttpClientTest());
#endif
            tests.Add(new Tests.WebRequestTest());

            var str = Console.ReadLine();
            var test = tests.Find(a => a.Name == str);
            var arg = Console.ReadLine();
            test.Test(new string[] { arg });

            Console.ReadKey();
        }

        /// <summary>
        /// 测试向下转型，无法将父类对象转换成子类对象
        /// 但是可以将指向子类对象的父类引用转型成子类引用
        /// </summary>
        public static void TestDownwardTransformation()
        {
            Dog animal = (Dog)new Animal();
            animal.Call();
        }
        public class Animal { public string Name; }
        public class Dog: Animal { public void Call() { } }

    }
}
