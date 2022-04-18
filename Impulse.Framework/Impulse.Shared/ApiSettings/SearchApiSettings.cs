// <copyright file="SearchApiSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.Datastructures;

namespace Impulse.Shared.ApiSettings;

public class SearchApiSettings
{
    public SearchApiSettings()
    {
        IsUsingIpBias = false;
    }

    // Setting radius to 0 will allow for an undefined radius.
    public int Radius { get; set; }

    // An alternate method of search biasing.
    public Location Location { get; set; }

    // Bias results using users IP.
    // When set to true, this will be instead of location biasing above.
    public bool IsUsingIpBias { get; set; }
}
