using System;
using System.Collections.Generic;
using System.Text;

namespace Serializer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            YAML.YAMLTest.Test();
            Console.WriteLine();
            JSON.JSONTest.Test();
            Console.WriteLine();
            XML.XMLTest.Test();
            Console.WriteLine();
            Props.PropertiesSerializerTest.Test();
            Console.ReadKey();
        }
    }
}
