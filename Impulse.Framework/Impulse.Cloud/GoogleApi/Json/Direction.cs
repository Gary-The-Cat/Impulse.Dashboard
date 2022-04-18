// <copyright file="Direction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Impulse.Cloud.GoogleApi.Json;

public class Direction
{
    public string Summary { get; set; }

    public List<Leg> Legs { get; set; }

    // Only used when returning optimised waypoints.
    public List<int> Waypoint_Order { get; set; }

    public Polyline Overview_Polyline { get; set; }

    public Viewport Bounds { get; set; }

    public string Copyrights { get; set; }

    public List<string> Warnings { get; set; }
}
