using System;

namespace Algorithms_Simulation.Services;

public class RandomArrayGenerator
{
    private static readonly Random Random = new();

    public int[] Generate(int length, int minValue, int maxValue)
    {
        if (length <= 0)
        {
            return Array.Empty<int>();
        }

        var values = new int[length];
        for (var i = 0; i < length; i++)
        {
            values[i] = Random.Next(minValue, maxValue + 1);
        }

        return values;
    }
}
