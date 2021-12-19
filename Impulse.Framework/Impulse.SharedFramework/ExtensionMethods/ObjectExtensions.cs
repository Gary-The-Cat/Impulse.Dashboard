// <copyright file="ObjectExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.SharedFramework.ExtensionMethods
{
    public static class ObjectExtensions
    {
        public static T GetPropertyValue<T>(this object src, string propName)
        {
            return (T)src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
