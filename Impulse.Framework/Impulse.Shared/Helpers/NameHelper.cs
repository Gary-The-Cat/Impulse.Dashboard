// <copyright file="NameHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Impulse.Shared.Helpers
{
    public enum UniqueNameMode
    {
        Numerical,
        Copy
    }

    public static class NameHelper
    {
        public static UniqueNameMode Mode { get; set; } = UniqueNameMode.Numerical;

        public static string GetUniqueName(IEnumerable<string> existingNames, string desiredName)
        {
            var existingNameHash = existingNames.ToHashSet();

            // Check if we can immediately return the requested name
            if (!existingNameHash.Contains(desiredName))
            {
                return desiredName;
            }

            if (Mode == UniqueNameMode.Numerical)
            {
                if (!existingNameHash.Contains(desiredName))
                {
                    return desiredName;
                }

                int currentValue = 2;

                var suggestedName = $"{desiredName}_{currentValue}";

                while (existingNameHash.Contains(suggestedName))
                {
                    currentValue++;
                    suggestedName = $"{desiredName}_{currentValue}";
                }

                return suggestedName;
            }

            if (Mode == UniqueNameMode.Copy)
            {
                var suggestedName = $"{desiredName} (Copy)";

                while (existingNameHash.Contains(suggestedName))
                {
                    suggestedName = $"{suggestedName} (Copy)";
                }

                return suggestedName;
            }

            throw new System.Exception("We were unable to find any unique name.");
        }
    }
}
