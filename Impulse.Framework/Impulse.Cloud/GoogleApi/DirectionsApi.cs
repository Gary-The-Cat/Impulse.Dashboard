// <copyright file="DirectionsApi.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Impulse.Cloud.GoogleApi.Json;
using Impulse.Shared.ApiSettings;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Enums;
using Newtonsoft.Json;

namespace Impulse.Cloud.GoogleApi;

public class DirectionsApi
{
    public DirectionsApi()
    {
        this.Settings = new DirectionsApiSettings
        {
            Avoid = DirectionsAvoid.none,
            Mode = DirectionsMode.driving,
            Units = Units.metric,
        };
    }

    public DirectionsApi(DirectionsApiSettings settings)
    {
        this.Settings = settings;
    }

    public DirectionsApiSettings Settings { get; set; }

    private string UriString
    {
        // Construct the URI string.
        // The origin address will be the first replace token and the
        // destination address the second replace token as part of String.Format().
        get
        {
            var uriString =
                $"{Keys.DirectionsApiUriString}{Keys.OutputFormat}?" +
                $"{Keys.ParameterOrigin}=" + "{0}&" +
                $"{Keys.ParameterDestination}=" + "{1}&" +
                $"{Keys.ParameterApiKey}={Keys.ApiKey}&" +
                $"{Keys.ParameterLanguage}=en-AU&" +
                $"{Keys.ParameterAlternatives}={this.Settings.Alternatives.ToLowerString()}&" +
                $"{Keys.ParameterMode}={this.Settings.Mode}&" +
                $"{Keys.ParameterUnits}={this.Settings.Units}";

            if (this.Settings.Avoid != DirectionsAvoid.none)
            {
                uriString +=
                    $"&{Keys.ParamaterAvoid}={this.Settings.Avoid}";
            }

            return uriString;
        }
    }

    public async Task<IEnumerable<DirectionInformation>> GetDirectionInformationAsync(
        string originAddress,
        string destinationAddress,
        WaypointMode mode,
        IList<string> waypoints = null)
    {
        var directionResponse = await this.ListDirectionsAsync(originAddress, destinationAddress, mode, waypoints);
        return directionResponse.Routes.Select(entry => new DirectionInformation
        {
            Summary = entry.Summary,
            Duration = entry.Legs.Sum(entry => entry.Duration.Value),
            Distance = entry.Legs.Sum(entry => entry.Distance.Value),
        });
    }

    public async Task<IEnumerable<string>> GetSpokenDirectionsAsync(
        string originAddress,
        string destinationAddress,
        WaypointMode mode,
        IList<string> waypoints = null)
    {
        var directionResponse = await this.ListDirectionsAsync(originAddress, destinationAddress, mode, waypoints);
        return directionResponse.Routes.SelectMany(r => r.Legs).SelectMany(l => l.Steps).Select(r => r.Html_Instructions);
    }

    private async Task<DirectionResponse> ListDirectionsAsync(
        string originAddress,
        string destinationAddress,
        WaypointMode mode,
        IList<string> waypoints = null)
    {
        if (mode == WaypointMode.PLACE_ID)
        {
            originAddress = $"place_id:{originAddress}";
            destinationAddress = $"place_id:{destinationAddress}";
        }

        var uriString = string.Format(this.UriString, originAddress, destinationAddress);
        if (waypoints != null && waypoints.Any())
        {
            // Add the waypoints into the request.
            uriString += $"&{Keys.ParameterWaypoints}=";
            var joinKey = "via:";
            if (this.Settings.OptimiseWaypoints)
            {
                uriString += $"{Keys.ParameterOptimise}:{this.Settings.OptimiseWaypoints.ToLowerString()}|";
                joinKey = string.Empty;
            }

            foreach (var waypoint in waypoints)
            {
                uriString += mode == WaypointMode.PLACE_ID ? $"{joinKey}place_id:" : joinKey;
                uriString += waypoint;

                if (waypoint != waypoints.Last())
                {
                    uriString += "|";
                }
            }
        }

        var stringResponse = await HttpHelper.Get(uriString);
        return JsonConvert.DeserializeObject<DirectionResponse>(stringResponse);
    }
}
