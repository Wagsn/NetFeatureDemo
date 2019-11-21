using System;
using System.Collections.Generic;
using System.Text;

namespace SerializerSharp
{
    public class SerializerFactory : ISerializerFactory
    {
        private Dictionary<string, ISerializer> Serializers { get; } = new Dictionary<string, ISerializer>();

        public ISerializer GetByName(string serializerName)
        {
            return Serializers[serializerName];
        }

        public void AddSerializer(ISerializer serializer)
        {
            Serializers[serializer.Name] = serializer;
        }
    }
}
