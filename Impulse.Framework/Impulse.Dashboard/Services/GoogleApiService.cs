// <copyright file="GoogleApiService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.Cloud.GoogleApi;
using Impulse.Shared.ApiSettings;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Enums;
using Impulse.Shared.Services;

namespace Impulse.Dashboard.Services;

public class GoogleApiService : IGoogleApiService
{
    private readonly PlacesApi placesApi;

    private readonly SearchApi searchApi;

    private readonly DirectionsApi directionsApi;

    public GoogleApiService()
    {
        placesApi = new PlacesApi(new PlacesApiSettings
        {
            CountryRestriction = "au",
            Types = PlaceType.any,
            SessionToken = Guid.NewGuid(),
        });

        searchApi = new SearchApi(new SearchApiSettings
        {
            IsUsingIpBias = true
        });

        directionsApi = new DirectionsApi(new DirectionsApiSettings
        {
            Avoid = DirectionsAvoid.none,
            Mode = DirectionsMode.driving,
            Units = Units.metric,
        });
    }

    public async Task<IEnumerable<DirectionInformation>> GetDirectionInformationAsync(
        string originAddress,
        string destinationAddess,
        WaypointMode mode)
    {
        return await directionsApi.GetDirectionInformationAsync(originAddress, destinationAddess, mode);
    }

    public async Task<IEnumerable<string>> GetSpokenDirectionsAsync(
        string originAddress,
        string destinationAddess,
        WaypointMode mode,
        IList<string> waypoints)
    {
        return await directionsApi.GetSpokenDirectionsAsync(originAddress, destinationAddess, mode);
    }

    public async Task<IEnumerable<DirectionInformation>> GetDirectionInformationAsync(
        string originAddress,
        string destinationAddess,
        WaypointMode mode,
        IList<string> waypoints)
    {
        return await directionsApi.GetDirectionInformationAsync(originAddress, destinationAddess, mode, waypoints);
    }

    public DirectionsApiSettings GetDirectionsApiSettings()
    {
        return this.directionsApi.Settings;
    }

    public PlacesApiSettings GetPlacesApiSettings()
    {
        return this.placesApi.Settings;
    }

    public SearchApiSettings GetSearchApiSettings()
    {
        return this.searchApi.Settings;
    }

    public async Task<IEnumerable<Address>> ListPlacePredictionAddressesAsync(string address)
    {
        return await placesApi.ListPlacePredictionAddressesAsync(address);
    }

    public async Task<IEnumerable<Address>> ListSearchAddressesAsync(string query)
    {
        return await searchApi.ListSearchAddressesAsync(query);
    }

    public void SetDirectionsApiSettings(DirectionsApiSettings settings)
    {
        this.directionsApi.Settings = settings;
    }

    public void SetPlacesApiSettings(PlacesApiSettings settings)
    {
        this.placesApi.Settings = settings;
    }

    public void SetSearchApiSettings(SearchApiSettings settings)
    {
        this.searchApi.Settings = settings;
    }
}
