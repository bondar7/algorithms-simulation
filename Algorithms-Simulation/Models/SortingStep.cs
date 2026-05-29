using System;
using System.Collections.Generic;

namespace Algorithms_Simulation.Models;

public class SortingStep
{
    public int[] ArrayState { get; init; } = Array.Empty<int>();
    public List<int> ComparingIndices { get; init; } = new();
    public List<int> SwappingIndices { get; init; } = new();
    public List<int> MovingIndices { get; init; } = new();
    public List<int> SortedIndices { get; init; } = new();
    public int? PivotIndex { get; init; }
    public int? CurrentIndex { get; init; }
    public int? TargetIndex { get; init; }
    public SortingOperation Operation { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsMajorStep { get; init; }
    public string? GroupTitle { get; init; }
    public int Comparisons { get; init; }
    public int Swaps { get; init; }
    public int Writes { get; init; }
}
