// <copyright file="PlacesApi.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Impulse.Cloud.GoogleApi.Json;
using Impulse.Shared.ApiSettings;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Enums;
using Newtonsoft.Json;

namespace Impulse.Cloud.GoogleApi;

public class PlacesApi
{
    public PlacesApi()
    {
        this.Settings = new PlacesApiSettings
        {
            Radius = 5000,
            SessionToken = Guid.NewGuid(),
            Types = PlaceType.any,
        };
    }

    public PlacesApi(PlacesApiSettings settings)
    {
        this.Settings = settings;
    }

    public PlacesApiSettings Settings { get; set; }

    private string UriString
    {
        // Construct the URI string.
        // The address will be the first replace token as part of String.Format().
        get
        {
            var uriString =
                $"{Keys.PlacesAutoCompleteApiUriString}{Keys.OutputFormat}?" +
                $"{Keys.ParameterInput}=" + "{0}&" +
                $"{Keys.ParameterApiKey}={Keys.ApiKey}&" +
                $"{Keys.ParameterLanguage}=en-AU";

            if (this.Settings.Location != null)
            {
                uriString +=
                    $"&{Keys.ParameterRadius}={this.Settings.Radius}" +
                    $"&{Keys.ParameterLocation}={this.Settings.Location.Lat},{this.Settings.Location.Lng}";
            }

            if (this.Settings.Origin != null)
            {
                uriString +=
                    $"&{Keys.ParameterOrigin}={this.Settings.Origin.Lat},{this.Settings.Origin.Lng}";
            }

            if (this.Settings.Types != PlaceType.any)
            {
                uriString +=
                    $"&{Keys.ParameterTypes}={this.Settings.Types}";
            }

            if (!string.IsNullOrEmpty(this.Settings.CountryRestriction))
            {
                uriString +=
                    $"&{Keys.ParameterComponents}=country:{this.Settings.CountryRestriction}";
            }

            if (this.Settings.SessionToken != Guid.Empty)
            {
                uriString +=
                    $"&{Keys.ParameterSessionToken}={this.Settings.SessionToken}";
            }

            return uriString;
        }
    }

    public async Task<IEnumerable<PlacePrediction>> ListPlacePredictionsAsync(string address)
    {
        var uriString = string.Format(this.UriString, address);
        var stringResponse = await HttpHelper.Get(uriString);

        var predictionResponse = JsonConvert.DeserializeObject<PredictionResponse>(stringResponse);
        return predictionResponse.Predictions;
    }

    public async Task<IEnumerable<Address>> ListPlacePredictionAddressesAsync(string address)
    {
        var predictions = await this.ListPlacePredictionsAsync(address);
        return predictions.Select(entry => new Address
        {
            FormattedAddress = entry.Description,
            PlaceId = entry.Place_Id
        });
    }
}
