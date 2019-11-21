﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SerializerSharp.JSON
{
    /// <summary>
    /// JSON序列化器
    /// </summary>
    public class Serializer : ISerializer
    {
        public TypeEntity Deserialize<TypeEntity>(string content)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TypeEntity>(content);
        }

        public TypeEntity Deserialize<TypeEntity>(Stream stream)
        {
            var reader = new StreamReader(stream);
            var entity = Deserialize<TypeEntity>(reader.ReadToEnd());
            reader.Close();
            return entity;
        }

        public string Serialize(object entity)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(entity);
        }

        public void Serialize(object entity, Stream stream)
        {
            var writer = new StreamWriter(stream);
            writer.Write(Serialize(entity));
            writer.Flush();
            writer.Close();
        }
    }
}