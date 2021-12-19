// <copyright file="SearchApi.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Impulse.Cloud.GoogleApi.Json;
using Impulse.Shared.ApiSettings;
using Impulse.Shared.Datastructures;
using Newtonsoft.Json;

namespace Impulse.Cloud.GoogleApi
{
    public class SearchApi
    {
        public SearchApi()
        {
            this.Settings = new SearchApiSettings
            {
                IsUsingIpBias = true
            };
        }

        public SearchApi(SearchApiSettings settings)
        {
            this.Settings = settings;
        }

        public SearchApiSettings Settings { get; set; }

        private string UriString
        {
            // Construct the URI string.
            // The query will be the first replace token as part of String.Format().
            get
            {
                var uriString =
                    $"{Keys.PlacesSearchApiUriString}{Keys.OutputFormat}?" +
                    $"{Keys.ParameterInput}=" + "{0}&" +
                    $"{Keys.ParameterInputType}={Keys.KeyInputType}&" +
                    $"{Keys.ParameterApiKey}={Keys.ApiKey}&" +
                    $"{Keys.ParameterLanguage}=en-AU&" +
                    $"{Keys.ParameterFields}={Keys.KeySearchFields}";

                if (this.Settings.IsUsingIpBias)
                {
                    uriString +=
                        $"&{Keys.ParameterLocationBias}={Keys.KeyIpBias}";
                }
                else if (this.Settings.Location != null)
                {
                    if (this.Settings.Radius > 0)
                    {
                        uriString +=
                            $"&{Keys.ParameterLocationBias}=circle:{this.Settings.Radius}@" +
                            $"{this.Settings.Location.Lat},{this.Settings.Location.Lng}";
                    }
                    else
                    {
                        uriString +=
                            $"&{Keys.ParameterLocationBias}=point:" +
                            $"{this.Settings.Location.Lat},{this.Settings.Location.Lng}";
                    }
                }

                return uriString;
            }
        }

        public async Task<IEnumerable<PlaceSearch>> ListSearchsAsync(string query)
        {
            var uriString = string.Format(this.UriString, query);
            var stringResponse = await HttpHelper.Get(uriString);

            var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(stringResponse);
            return searchResponse.Candidates;
        }

        public async Task<IEnumerable<Address>> ListSearchAddressesAsync(string query)
        {
            var places = await this.ListSearchsAsync(query);
            return places.Select(entry => new Address
            {
                FormattedAddress = entry.Formatted_Address,
                PlaceId = entry.Place_Id
            });
        }
    }
}
