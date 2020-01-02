using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PluginCore
{
    /// <summary>
    /// 文件夹程序集加载器
    /// </summary>
    public class DirectoryLoader
    {
        /// <summary>
        /// 从文件夹中加载程序集
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public virtual List<Assembly> LoadFromDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return new List<Assembly>();
            }
            List<Assembly> assemblyList = new List<Assembly>();
            // 加载程序集信息
            string[] dllFiles = Directory.GetFiles(dir, "*.dll");
            foreach (string file in dllFiles)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    assemblyList.Add(assembly);
                }
                catch
                {
                    //Console.WriteLine
                }
            }
            return assemblyList;
        }

        /// <summary>
        /// 查询程序集中所有以baseType为基类并满足checker的类
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="baseType"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public virtual List<Type> GetTypes(Assembly assembly, Type baseType, Func<Type, bool> checker)
        {
            Func<Type, bool> actualCheck = (ti) =>
            {
                bool isOk = true;
                if (baseType != null)
                {
                    if (baseType.GetTypeInfo().IsAssignableFrom(ti))
                    {
                        isOk = true;
                    }
                    else
                    {
                        isOk = false;
                    }
                }

                if (isOk)
                {
                    if (checker != null)
                    {
                        isOk = checker(ti);
                    }
                }
                return isOk;
            };
            return assembly.GetTypes().Where(actualCheck).ToList();
        }
    }
}
