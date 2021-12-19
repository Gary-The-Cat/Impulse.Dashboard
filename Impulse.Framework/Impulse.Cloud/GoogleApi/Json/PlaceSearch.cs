// <copyright file="PlaceSearch.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Cloud.GoogleApi.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Impulse.Cloud.GoogleApi.Json
{
    public class PlaceSearch
    {
        public string Name { get; set; }

        public string Formatted_Address { get; set; }

        public Geometry Geometry { get; set; }

        public string Icon { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessStatus Business_Status { get; set; }

        // A textual identifier that uniquely identifies a place
        public string Place_Id { get; set; }

        public PlusCode Plus_Code { get; set; }

        public string Next_Page_Token { get; set; }
    }

    public class PlusCode
    {
        public string Global_Code { get; set; }

        public string Compound_Code { get; set; }
    }
}
