using System;
using System.Collections.Generic;
using System.Text;

namespace SerializerSharp
{
    internal class CSVTest
    {
        public static void Test()
        {
            // CsvHelper.CsvReader 只支持流
            var source = "Wagsn,23,true\r\nBruce,25,true";
            using (CsvHelper.CsvReader reader = new CsvHelper.CsvReader(new System.IO.StreamReader(ConvertHelper.ConvertToStream(source))))
            {
                if (reader.Read())
                {
                    foreach(var per in reader.GetRecords<Person>())
                    {
                        Console.WriteLine($"deserialize: name: {per?.Name}, age: {per?.Age}, sex: {per?.Sex}");
                    }
                }
            }
            //var path = "./CSV/person.csv";
        }
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public bool Sex { get; set; }
        }
    }
}
