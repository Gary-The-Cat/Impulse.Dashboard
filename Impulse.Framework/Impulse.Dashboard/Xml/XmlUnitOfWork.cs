// <copyright file="XmlUnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Impulse.Shared.Interfaces;
using Impulse.Shared.Serialization;
using Impulse.SharedFramework.Services;

namespace Impulse.Dashboard.Xml
{
    public class XmlUnitOfWork : IUnitOfWork
    {
        private readonly string sourceFile;
        private readonly XElement root;
        private readonly IDeserializer[] deserializers;
        private readonly ISerializer[] serializers;

        public XmlUnitOfWork(
            XElement root,
            string sourceFile,
            IDeserializer[] deserializers,
            ISerializer[] serializers)
        {
            this.root = root;
            this.sourceFile = sourceFile;
            this.deserializers = deserializers;
            this.serializers = serializers;
        }

        public async Task<T> LoadAsync<T>(Guid id) where T : IHaveId, new()
        {
            foreach (var element in root.Elements())
            {
                var type = element.Attribute(GlobalSerializationKeys.Type).Value;

                if (type == null)
                {
                    continue;
                }

                if (type.Equals(typeof(T).FullName))
                {
                    var xId = XmlConvert.ToGuid(element.Attribute(GlobalSerializationKeys.Id).Value);

                    if (xId == id)
                    {
                        var deserializer = deserializers.First(d => d.CanHandle(typeof(T)));
                        return (T)deserializer.Deserialize(element);
                    }
                }
            }

            return default(T);
        }

        public async Task<List<T>> LoadAllAsync<T>() where T : IHaveId, new()
        {
            var values = new List<T>();

            foreach (var element in root.Elements())
            {
                var type = element.Attribute(GlobalSerializationKeys.Type).Value;

                if (type == null)
                {
                    continue;
                }

                if (type.Equals(typeof(T).FullName))
                {
                    var deserializer = deserializers.First(d => d.CanHandle(typeof(T)));
                    values.Add((T)deserializer.Deserialize(element));
                }
            }

            return values;
        }

        public async Task StoreAsync<T>(T t) where T : IHaveId, new()
        {
            await DeleteAsync(t);

            var serializer = serializers.First(s => s.CanHandle(t));
            root.Add(serializer.Serialize(t));
        }

        public async Task SaveChangesAsync()
        {
            File.WriteAllText(sourceFile, root.ToString());
        }

        public async Task DeleteAsync<T>(T t) where T : IHaveId, new()
        {
            var element = root.Elements()
                .FirstOrDefault(e => XmlConvert.ToGuid(e.Attribute(GlobalSerializationKeys.Id).Value) == t.Id);

            element?.Remove();
        }

        public void Dispose()
        {
            // Disposal is currently done directly after reading from the data source, this wouldn't
            // be the case if the data source was something more complex.
        }
    }
}
