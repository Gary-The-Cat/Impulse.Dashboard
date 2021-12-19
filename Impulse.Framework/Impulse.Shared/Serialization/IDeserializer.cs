// <copyright file="IDeserializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Xml.Linq;
using Impulse.Shared.Interfaces;

namespace Impulse.Shared.Serialization
{
    public interface IDeserializer : IAmKernelInjected
    {
        bool CanHandle(Type type);

        object Deserialize(XElement xElement);
    }
}
