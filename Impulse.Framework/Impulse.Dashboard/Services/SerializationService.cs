// <copyright file="SerializationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Impulse.Dashboard.Xml;
using Impulse.Shared.Serialization;
using Impulse.SharedFramework.Services;

namespace Impulse.Dashboard.Services
{
    public class SerializationService : ISerializationService
    {
        private readonly IDeserializer[] deserializers;
        private readonly ISerializer[] serializers;
        private readonly Dictionary<string, XElement> inMemoryDatabase;
        private Func<string> getSource;

        public SerializationService(ISerializer[] serializers, IDeserializer[] deserializers)
        {
            this.serializers = serializers;
            this.deserializers = deserializers;
            inMemoryDatabase = new Dictionary<string, XElement>();
        }

        public void Flush()
        {
            inMemoryDatabase.Clear();
        }

        public IUnitOfWork GetUnitOfWork()
        {
            var source = getSource();
            if (!string.IsNullOrWhiteSpace(source))
            {
                return GetUnitOfWork(source);
            }

            return null;
        }

        public IUnitOfWork GetUnitOfWork(string xmlFileSource)
        {
            XElement xRoot;

            // If the database does not currently contain the item, load it from file or apply the default
            if (!inMemoryDatabase.TryGetValue(xmlFileSource, out xRoot))
            {
                // Get the root element
                if (!File.Exists(xmlFileSource))
                {
                    File.WriteAllText(xmlFileSource, string.Empty);
                }

                var xmlSourceText = File.ReadAllText(xmlFileSource);

                var source = string.IsNullOrEmpty(xmlSourceText)
                    ? new XDocument()
                    : XDocument.Parse(xmlSourceText);

                xRoot = source.Root ?? new XElement("Root");

                inMemoryDatabase.Add(xmlFileSource, xRoot);
            }

            return new XmlUnitOfWork(
                xRoot,
                xmlFileSource,
                deserializers,
                serializers);
        }

        public void GetDefaultSource(Func<string> getSource)
        {
            this.getSource = getSource;
        }
    }
}