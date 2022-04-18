// <copyright file="PlacePrediction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Impulse.Cloud.GoogleApi.Json;

public class PlacePrediction
{
    // Contains the human-readable name for the returned result
    public string Description { get; set; }

    // Contains an integer indicating the straight-line distance between the
    // predicted place, and the specified origin point, in meters
    public int DistanceInMetres { get; set; }

    public string Id { get; set; }

    // A textual identifier that uniquely identifies a place
    public string Place_Id { get; set; }

    public string Reference { get; set; }

    // These describe the location of the entered term in the prediction result text,
    // so that the term can be highlighted if desired
    public List<MatchedSubstrings> MatchedSubstrings { get; set; }

    // Provides pre-formatted text that can be shown in your autocomplete results
    public StructuredFormatting StructuredFormatting { get; set; }

    // Contains an array of terms identifying each section of the returned description
    // (a section of the description is generally terminated with a comma)
    public List<PlaceTerm> Terms { get; set; }

    // Contains an array of types that apply to this place
    public List<string> Types { get; set; }
}
