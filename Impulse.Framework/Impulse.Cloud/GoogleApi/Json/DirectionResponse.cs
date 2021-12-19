// <copyright file="DirectionResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Impulse.Cloud.GoogleApi.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Impulse.Cloud.GoogleApi.Json
{
    public class DirectionResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusResponse Status { get; set; }

        public List<Direction> Routes { get; set; }
    }
}
