using System.Collections.Generic;
using Algorithms_Simulation.Models;

namespace Algorithms_Simulation.Algorithms;

public interface ISortingAlgorithm
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    string TimeComplexity { get; }
    string SpaceComplexity { get; }
    string Pseudocode { get; }

    IReadOnlyList<SortingStep> GenerateSteps(int[] input);
}
