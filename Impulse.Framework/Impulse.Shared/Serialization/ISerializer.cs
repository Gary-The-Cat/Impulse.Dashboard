// <copyright file="ISerializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Xml.Linq;
using Impulse.Shared.Interfaces;

namespace Impulse.Shared.Serialization
{
    public interface ISerializer : IAmKernelInjected
    {
        bool CanHandle<T>(T t);

        XElement Serialize(object instance);
    }
}
