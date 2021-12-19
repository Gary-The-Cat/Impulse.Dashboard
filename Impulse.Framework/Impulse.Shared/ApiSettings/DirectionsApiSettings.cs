// <copyright file="DirectionsApiSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.Enums;

namespace Impulse.Shared.ApiSettings
{
    public class DirectionsApiSettings
    {
        public DirectionsMode Mode { get; set; }

        public bool OptimiseWaypoints { get; set; }

        public bool Alternatives { get; set; }

        public DirectionsAvoid Avoid { get; set; }

        public Units Units { get; set; }
    }
}
