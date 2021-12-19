// <copyright file="Step.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.Datastructures;

namespace Impulse.Cloud.GoogleApi.Json
{
    public class Step
    {
        public string Html_Instructions { get; set; }

        // Value is distance in metres.
        public ValueEntry Distance { get; set; }

        // Value is duration in seconds.
        public ValueEntry Duration { get; set; }

        public Location Start_Location { get; set; }

        public Location End_Location { get; set; }

        public string Maneuver { get; set; }

        public Polyline Polyline { get; set; }
    }
}
