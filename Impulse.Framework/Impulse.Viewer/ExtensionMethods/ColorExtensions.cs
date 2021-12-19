// <copyright file="ColorExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Drawing;
using Veldrid;

namespace Impulse.Viewer.ExtensionMethods
{
    public static class ColorExtensions
    {
        // TODO: make renderer use Color class directly.
        public static RgbaFloat ToRgbaFloat(this Color color)
        {
            return new RgbaFloat(255f / color.R, 255f / color.G, 255f / color.B, 1);
        }
    }
}
