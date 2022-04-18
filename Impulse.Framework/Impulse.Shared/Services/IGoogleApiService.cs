// <copyright file="IGoogleApiService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.Shared.ApiSettings;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Enums;

namespace Impulse.Shared.Services;

public interface IGoogleApiService
{
    PlacesApiSettings GetPlacesApiSettings();

    void SetPlacesApiSettings(PlacesApiSettings settings);

    // List place predictions based on a supplied address.
    // This uses the places autocomplete api.
    Task<IEnumerable<Address>> ListPlacePredictionAddressesAsync(string address);

    SearchApiSettings GetSearchApiSettings();

    void SetSearchApiSettings(SearchApiSettings settings);

    // List place searchs based on a supplied address.
    // This uses the places search api.
    Task<IEnumerable<Address>> ListSearchAddressesAsync(string query);

    DirectionsApiSettings GetDirectionsApiSettings();

    void SetDirectionsApiSettings(DirectionsApiSettings settings);

    // Get directions for the addresses supplied.
    // If text input addresses are used, set waypoint mode to ADDRESS.
    // If the addresses are instead place ids, set waypoint mode to PLACE_ID
    // This uses the directions api.
    Task<IEnumerable<string>> GetSpokenDirectionsAsync(
        string originAddress,
        string destinationAddess,
        WaypointMode mode,
        IList<string> waypoints);

    // Get directions for the addresses supplied.
    // If text input addresses are used, set waypoint mode to ADDRESS.
    // If the addresses are instead place ids, set waypoint mode to PLACE_ID
    // This uses the directions api.
    Task<IEnumerable<DirectionInformation>> GetDirectionInformationAsync(
        string originAddress,
        string destinationAddess,
        WaypointMode mode);

    // Same as above, but includes stops along the way, i.e the waypoints.
    // Note the waypoint addresses need to be the same mode (address or place_id) as
    // the input addresses.
    Task<IEnumerable<DirectionInformation>> GetDirectionInformationAsync(
        string originAddress,
        string destinationAddess,
        WaypointMode mode,
        IList<string> waypoints);
}
