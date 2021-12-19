// <copyright file="WeakReferenceExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.Shared.ExtensionMethods
{
    public static class WeakReferenceExtensions
    {
        public static T Value<T>(this WeakReference<T> weakReference) where T : class
        {
            if (weakReference.TryGetTarget(out var value))
            {
                return value;
            }

            return default(T);
        }
    }
}
