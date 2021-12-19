// <copyright file="TypeExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;

namespace Impulse.Shared.ExtensionMethods
{
    public static class TypeExtensions
    {
        public static bool Implements(this Type type, Type interfaceType)
        {
            if (type.GetInterfaces().Any(i => i == interfaceType))
            {
                return true;
            }

            return false;
        }
    }
}
