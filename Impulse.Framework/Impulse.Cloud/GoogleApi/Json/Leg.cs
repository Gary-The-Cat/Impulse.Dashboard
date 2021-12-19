// <copyright file="Legs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Impulse.Shared.Datastructures;

namespace Impulse.Cloud.GoogleApi.Json
{
    public class Leg
    {
        public List<Step> Steps { get; set; }

        // Value is distance in metres.
        public ValueEntry Distance { get; set; }

        // Value is duration in seconds.
        public ValueEntry Duration { get; set; }

        public Time Arrival_Time { get; set; }

        public Time Departure_Time { get; set; }

        public Location Start_Location { get; set; }

        public Location End_Location { get; set; }

        public string Start_Address { get; set; }

        public string End_Address { get; set; }
    }
}
