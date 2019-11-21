using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SerializerSharp
{
    /// <summary>
    /// CSV Serializer
    /// </summary>
    public class CsvSerializer : ISerializer
    {
        public string Name { get; set; } = "CsvHelper";

        public TypeEntity Deserialize<TypeEntity>(string content)
        {
            throw new NotImplementedException();
        }

        public TypeEntity Deserialize<TypeEntity>(Stream stream)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object entity)
        {
            throw new NotImplementedException();
        }

        public void Serialize(object entity, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
