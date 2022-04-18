// <copyright file="MonoGameServiceProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Impulse.Mono.Viewer.MonoGameWrapper;

public class MonoGameServiceProvider : IServiceProvider
{
    private readonly Dictionary<Type, object> services;

    public MonoGameServiceProvider()
    {
        services = new Dictionary<Type, object>();
    }

    public void AddService(Type type, object provider)
    {
        services.Add(type, provider);
    }

    public object GetService(Type type)
    {
        if (services.TryGetValue(type, out var service))
        {
            return service;
        }

        return null;
    }

    public void RemoveService(Type type)
    {
        services.Remove(type);
    }

    public void AddService<T>(T service)
    {
        AddService(typeof(T), service);
    }

    public T GetService<T>() where T : class
    {
        var service = GetService(typeof(T));
        return (T)service;
    }
}
