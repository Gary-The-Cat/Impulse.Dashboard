namespace Impulse.SharedFramework.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;

public static class UniqueNameGenerator
{
    private const string DefaultName = "New Item";

    public static string GenerateUniqueName(
        IEnumerable<string> existingNames,
        string? proposedName,
        UniqueNameStrategy strategy = UniqueNameStrategy.NumberInParentheses,
        string? fallbackName = null)
    {
        if (existingNames == null)
        {
            throw new ArgumentNullException(nameof(existingNames));
        }

        var trimmedExisting = new HashSet<string>(
            existingNames
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim()),
            StringComparer.OrdinalIgnoreCase);

        var baseName = string.IsNullOrWhiteSpace(proposedName)
            ? fallbackName?.Trim()
            : proposedName.Trim();

        if (string.IsNullOrEmpty(baseName))
        {
            baseName = string.IsNullOrWhiteSpace(fallbackName) ? DefaultName : fallbackName.Trim();
        }

        if (!trimmedExisting.Contains(baseName))
        {
            return baseName;
        }

        var index = 1;
        string candidate;

        do
        {
            candidate = strategy switch
            {
                UniqueNameStrategy.DashNumber => $"{baseName}-{index}",
                UniqueNameStrategy.NumberInParentheses => $"{baseName} ({index})",
                _ => $"{baseName} ({index})",
            };

            index++;
        }
        while (trimmedExisting.Contains(candidate));

        return candidate;
    }
}
