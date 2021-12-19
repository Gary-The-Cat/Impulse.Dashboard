// <copyright file="SearchResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Impulse.Cloud.GoogleApi.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Impulse.Cloud.GoogleApi.Json
{
    public class SearchResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusResponse Status { get; set; }

        public List<string> HtmlAttributes { get; set; }

        public string NextPageToken { get; set; }

        public List<PlaceSearch> Candidates { get; set; }
    }
}
