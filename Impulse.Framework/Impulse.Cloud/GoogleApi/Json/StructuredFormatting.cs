// <copyright file="StructuredFormatting.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Impulse.Cloud.GoogleApi.Json
{
    public class StructuredFormatting
    {
        // Contains the main text of a prediction, usually the name of the place.
        public string MainText { get; set; }

        public List<MatchedSubstrings> MainTextMatchedSubstrings { get; set; }

        // Contains the secondary text of a prediction, usually the location of the place
        public string SecondaryText { get; set; }
    }
}
