// <copyright file="PluginLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Impulse.Dashboard.ExtensionMethods;

namespace Impulse.Dashboard.AppBootstrapper;

public static class PluginLoader
{
    public static IEnumerable<(Assembly assembly, Type type)> GetAllInstances<T>(
        IEnumerable<string> searchDirectories) =>
        searchDirectories
            .Where(Directory.Exists)
            .SelectMany(GetAllTypes)
            .Where(t => t.Item2.Implements(typeof(T)));

    public static IEnumerable<(Assembly assembly, Type type)> GetAllTypes(string searchDirectory) =>
            GetFilesWithDllExtension(searchDirectory)
            .Select(LoadAssemblyFromDll)
            .Where(CheckObjectNotNull)
            .Distinct()
            .SelectMany(GetTypesFromAssembly);

    private static bool CheckObjectNotNull(object obj) => obj is not null;

    private static IEnumerable<string> GetFilesWithDllExtension(string directory) =>
        Directory.GetFiles(directory, "*.dll");

    private static Assembly? LoadAssemblyFromDll(string dllPath)
    {
        try
        {
            return Assembly.LoadFrom(dllPath);
        }
        catch (Exception e) when (e is BadImageFormatException || e is FileLoadException)
        {
            return null;
        }
    }

    private static IEnumerable<(Assembly, Type)> GetTypesFromAssembly(Assembly assembly)
    {
        try
        {
            return assembly.GetExportedTypes().Select(type => (assembly, type));
        }
        catch (Exception e) when (e is BadImageFormatException || e is TypeLoadException)
        {
            return Enumerable.Empty<(Assembly, Type)>();
        }
    }
}
