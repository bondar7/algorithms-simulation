using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms_Simulation.Services;

public class ArrayParser
{
    public bool TryParse(string input, int maxLength, out int[] values, out string errorMessage)
    {
        values = Array.Empty<int>();
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Zadejte pros?m seznam cel?ch ??sel odd?len?ch ??rkami.";
            return false;
        }

        var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0)
        {
            errorMessage = "Zadejte pros?m seznam cel?ch ??sel odd?len?ch ??rkami.";
            return false;
        }

        if (parts.Length > maxLength)
        {
            errorMessage = $"Zadejte pros?m nejv??e {maxLength} ??sel.";
            return false;
        }

        var numbers = new List<int>();
        foreach (var part in parts)
        {
            if (!int.TryParse(part, out var value))
            {
                errorMessage = $"'{part}' nen? platn? cel? ??slo.";
                return false;
            }

            numbers.Add(value);
        }

        values = numbers.ToArray();
        return true;
    }
}
