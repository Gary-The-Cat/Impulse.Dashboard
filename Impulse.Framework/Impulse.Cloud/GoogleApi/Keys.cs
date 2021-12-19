// <copyright file="Keys.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.Cloud.GoogleApi
{
    public static class Keys
    {
        public const string ApiKey = "YOUR_KEY_GOES_HERE";

        // URI string information.
        public const string PlacesAutoCompleteApiUriString = "https://maps.googleapis.com/maps/api/place/autocomplete/";
        public const string PlacesSearchApiUriString = "https://maps.googleapis.com/maps/api/place/findplacefromtext/";
        public const string DirectionsApiUriString = "https://maps.googleapis.com/maps/api/directions/";
        public const string OutputFormat = "json";

        // Required parameters for all api's.
        public const string ParameterInput = "input";
        public const string ParameterApiKey = "key";

        // Required for the search api
        public const string ParameterInputType = "inputtype";
        public const string KeyInputType = "textquery";

        // Required for the directions api
        public const string ParameterOrigin = "origin";
        public const string ParameterDestination = "destination";

        // Optional autocomplete api parameters
        public const string ParameterSessionToken = "sessiontoken";
        public const string ParameterOffset = "offset";
        public const string ParameterLocation = "location";
        public const string ParameterRadius = "radius";
        public const string ParameterLanguage = "language";
        public const string ParameterTypes = "types";
        public const string ParameterComponents = "components";
        public const string ParameterStrictBounds = "strictbounds";

        // Optional search api parameters
        public const string ParameterFields = "fields";
        public const string KeySearchFields = "formatted_address,name,geometry,icon,business_status,plus_code,place_id";
        public const string ParameterLocationBias = "locationbias";
        public const string KeyIpBias = "ipbias";
        public const string KeyPoint = "point";

        // Optional direction api parameters
        public const string ParameterMode = "mode";
        public const string ParameterUnits = "units";
        public const string ParameterWaypoints = "waypoints";
        public const string ParameterAlternatives = "alternatives";
        public const string ParamaterAvoid = "avoid";
        public const string ParameterOptimise = "optimize";
    }
}
