// <copyright file="RgbaFloatExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Veldrid;

namespace Impulse.Viewer.ExtensionMethods
{
    public static class RgbaFloatExtensions
    {
        public static RgbaFloat SetIntensity(this RgbaFloat colour, float intensity)
        {
            return new RgbaFloat(
                colour.R * intensity,
                colour.G * intensity,
                colour.B * intensity,
                colour.A);
        }

        public static RgbaFloat SetOpacity(this RgbaFloat colour, float opacity)
        {
            return new RgbaFloat(
                colour.R,
                colour.G,
                colour.B,
                colour.A * opacity);
        }
    }
}
