using System;
using System.Collections.Generic;
using System.Text;

namespace SerializerSharpDemo
{
    class SerializerTest
    {
        public static void Test()
        {
            JsonTest();
        }

        public static void JsonTest()
        {
            var ser = new SerializerSharp.JSON.Serializer();
            var per = new Person { Name = "Wagsn", Age = 23, Sex = true };
            Console.WriteLine("JSON:");
            var content = ser.Serialize(per);
            Console.WriteLine("Serialize: " + content);
            var entity = ser.Deserialize<Person>(content);
            Console.WriteLine($"Deserialize: name={entity.Name}, " + entity);
            entity.Age++;
            Console.WriteLine("Serialize after Modified value: " + ser.Serialize(entity));
            var entityFromFile = ser.Deserialize<Person>(System.IO.File.OpenRead("person.json"));
            Console.WriteLine($"Deserialize from json file: name={entityFromFile.Name}, " + entityFromFile);
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public bool Sex { get; set; }
        }
    }

}
