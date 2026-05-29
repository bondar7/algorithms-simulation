using System;
using System.Collections.Generic;
using System.Linq;
using Algorithms_Simulation.Models;

namespace Algorithms_Simulation.Algorithms;

public class BubbleSortAlgorithm : ISortingAlgorithm
{
    public string Id => "bubble";
    public string Name => "Bubble Sort";
    public string Description => "Opakovane porovnava sousedni prvky a provadi swap, pokud jsou ve spatnem poradi.";
    public string TimeComplexity => "Nejlepsi: O(n), Prumer: O(n^2), Nejhorsi: O(n^2)";
    public string SpaceComplexity => "O(1)";
    public string Pseudocode => "for i = 0 to n-1\n  for j = 0 to n-i-2\n    if a[j] > a[j+1]\n      swap";

    public IReadOnlyList<SortingStep> GenerateSteps(int[] input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        var array = input.ToArray();
        var steps = new List<SortingStep>();
        var comparisons = 0;
        var swaps = 0;
        var writes = 0;

        steps.Add(CreateStep(
            array,
            SortingOperation.None,
            "Start",
            "Zacina Bubble Sort.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Pocatecni stav",
            sortedIndices: BuildSortedIndices(array.Length, 0)));

        for (var i = 0; i < array.Length; i++)
        {
            for (var j = 0; j < array.Length - i - 1; j++)
            {
                comparisons++;
                steps.Add(CreateStep(
                    array,
                    SortingOperation.Compare,
                    "Porovn?n?",
                    $"Porovn?n? {array[j]} a {array[j + 1]}.",
                    comparisons,
                    swaps,
                    writes,
                    comparingIndices: new[] { j, j + 1 },
                    sortedIndices: BuildSortedIndices(array.Length, i)));

                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    swaps++;
                    writes += 2;

                    steps.Add(CreateStep(
                        array,
                        SortingOperation.Swap,
                        "Swap",
                    $"Provadi se swap {array[j + 1]} a {array[j]}, protoze jsou ve spatnem poradi.",
                        comparisons,
                        swaps,
                        writes,
                        swappingIndices: new[] { j, j + 1 },
                        sortedIndices: BuildSortedIndices(array.Length, i)));
                }
            }

            steps.Add(CreateStep(
                array,
                SortingOperation.MarkSorted,
                "Oznaceni serazeneho",
                $"Prvek na indexu {array.Length - i - 1} je nyni na spravnem miste.",
                comparisons,
                swaps,
                writes,
                isMajorStep: true,
                groupTitle: $"Po pruchodu {i + 1}",
                sortedIndices: BuildSortedIndices(array.Length, i + 1)));
        }

        steps.Add(CreateStep(
            array,
            SortingOperation.Finished,
            "Hotovo",
            "Bubble Sort dokonceno.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Hotovo",
            sortedIndices: BuildSortedIndices(array.Length, array.Length)));

        return steps;
    }

    private static SortingStep CreateStep(
        int[] array,
        SortingOperation operation,
        string title,
        string description,
        int comparisons,
        int swaps,
        int writes,
        bool isMajorStep = false,
        string? groupTitle = null,
        IEnumerable<int>? comparingIndices = null,
        IEnumerable<int>? swappingIndices = null,
        IEnumerable<int>? sortedIndices = null)
    {
        return new SortingStep
        {
            ArrayState = array.ToArray(),
            Operation = operation,
            Title = title,
            Description = description,
            IsMajorStep = isMajorStep,
            GroupTitle = groupTitle,
            Comparisons = comparisons,
            Swaps = swaps,
            Writes = writes,
            ComparingIndices = comparingIndices?.ToList() ?? new List<int>(),
            SwappingIndices = swappingIndices?.ToList() ?? new List<int>(),
            SortedIndices = sortedIndices?.ToList() ?? new List<int>()
        };
    }

    private static IEnumerable<int> BuildSortedIndices(int length, int sortedCount)
    {
        if (sortedCount <= 0)
        {
            return Array.Empty<int>();
        }

        var start = Math.Max(0, length - sortedCount);
        return Enumerable.Range(start, length - start);
    }
}
