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
    public static List<(Assembly assembly, Type type)> GetAllInstances<T>(IEnumerable<string> searchDirectories)
    {
        var types = new List<(Assembly assembly, Type type)>();
        foreach (var dir in searchDirectories)
        {
            types.AddRange(GetAllTypes(dir).Where(t => t.type.Implements(typeof(T))));
        }

        return types;
    }

    public static List<(Assembly assembly, Type type)> GetAllTypes(string directory)
    {
        var applicationDllPaths = Directory.GetFiles(directory, "*.dll");

        var instances = new List<(Assembly, Type)>();

        // Loop over every dll in the folder, grab the types from the assembly, and check to see if any
        // of them implement our ApplicationInstance interface, if they do, grab a copy of the type.
        foreach (var dllPath in applicationDllPaths)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(dllPath);
            }
            catch
            {
                continue;
            }

            Type[] types;

            try
            {
                types = assembly.GetExportedTypes();
            }
            catch (Exception e)
            {
                continue;
            }

            foreach (var type in types)
            {
                instances.Add((assembly, type));
            }
        }

        return instances;
    }

    private static string GetRelativeApplicationDirectory()
    {
        var directory = Directory.GetCurrentDirectory();

        if (Directory.Exists(directory))
        {
            return directory;
        }

        throw new Exception("Application directory cannot be found.");
    }
}
