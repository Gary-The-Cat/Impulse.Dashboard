// <copyright file="PlacesApiSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Enums;

namespace Impulse.Shared.ApiSettings
{
    public class PlacesApiSettings
    {
        public PlacesApiSettings()
        {
            this.Radius = 5000;
        }

        // Radius and Location work together to bias search information
        // close to a target point.
        public int Radius { get; set; }

        public Location Location { get; set; }

        // This will restrict the search results to a target country.
        public string CountryRestriction { get; set; }

        // The current billing session.
        // If not set, each request is billed independently.
        public Guid SessionToken { get; set; }

        // The point at which to calculate distance.
        public Location Origin { get; set; }

        // The types of place results to return.
        public PlaceType Types { get; set; }
    }
}
