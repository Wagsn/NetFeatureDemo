using System;
using System.Collections.Generic;
using System.Text;

namespace SerializerSharp
{
    public class SerializerFactory : ISerializerFactory
    {
        private Dictionary<string, ISerializer> Serializers { get; } = new Dictionary<string, ISerializer>();

        public SerializerFactory()
        {
            AddSerializer(new JSON.JsonSerializer());
        }

        public ISerializer GetByName(string serializerName)
        {
            return Serializers[serializerName];
        }

        public void AddSerializer(ISerializer serializer)
        {
            Serializers[serializer.Name] = serializer;
        }

        public void AddSerializer<TSerializer>() where TSerializer: ISerializer, new()
        {
            var serializer = new TSerializer();
            Serializers[serializer.Name] = serializer;
        }
    }
}
