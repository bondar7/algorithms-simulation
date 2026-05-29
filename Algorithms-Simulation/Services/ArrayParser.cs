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
            errorMessage = "Zadejte prosim seznam celych cisel oddelenych carkami.";
            return false;
        }

        var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0)
        {
            errorMessage = "Zadejte prosim seznam celych cisel oddelenych carkami.";
            return false;
        }

        if (parts.Length > maxLength)
        {
            errorMessage = $"Zadejte prosim nejvyse {maxLength} cisel.";
            return false;
        }

        var numbers = new List<int>();
        foreach (var part in parts)
        {
            if (!int.TryParse(part, out var value))
            {
                errorMessage = $"'{part}' neni platne cele cislo.";
                return false;
            }

            numbers.Add(value);
        }

        values = numbers.ToArray();
        return true;
    }
}
