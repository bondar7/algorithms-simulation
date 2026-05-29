using System.Collections.Generic;

namespace Algorithms_Simulation.Algorithms;

public static class SortingAlgorithmRegistry
{
    public static IReadOnlyList<ISortingAlgorithm> GetAlgorithms()
    {
        return new List<ISortingAlgorithm>
        {
            new BubbleSortAlgorithm(),
            new SelectionSortAlgorithm(),
            new InsertionSortAlgorithm(),
            new MergeSortAlgorithm(),
            new QuickSortAlgorithm()
        };
    }
}
