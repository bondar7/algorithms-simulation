using System.Collections.Generic;
using System.Linq;
using Algorithms_Simulation.Models;

namespace Algorithms_Simulation.Algorithms;

public class SelectionSortAlgorithm : ISortingAlgorithm
{
    public string Id => "selection";
    public string Name => "Selection Sort";
    public string Description => "Postupne vybira nejmensi prvek z neserazene casti a presouva ho na spravne misto.";
    public string TimeComplexity => "Nejlepsi: O(n^2), Prumer: O(n^2), Nejhorsi: O(n^2)";
    public string SpaceComplexity => "O(1)";
    public string Pseudocode => "for i = 0 to n-1\n  min = i\n  for j = i+1 to n-1\n    if a[j] < a[min]\n      min = j\n  swap a[i], a[min]";

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
            "Zacina Selection Sort.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Pocatecni stav"));

        for (var i = 0; i < array.Length; i++)
        {
            var minIndex = i;

            steps.Add(CreateStep(
                array,
                SortingOperation.SelectMinimum,
                "V?b?r minima",
                "Hledame nejmensi prvek pro pozici {i}.",
                comparisons,
                swaps,
                writes,
                currentIndex: i,
                targetIndex: minIndex));

            for (var j = i + 1; j < array.Length; j++)
            {
                comparisons++;
                steps.Add(CreateStep(
                    array,
                    SortingOperation.Compare,
                    "Porovnani",
                    $"Porovnani minima {array[minIndex]} s {array[j]}.",
                    comparisons,
                    swaps,
                    writes,
                    comparingIndices: new[] { minIndex, j },
                    currentIndex: i,
                    targetIndex: minIndex));

                if (array[j] < array[minIndex])
                {
                    minIndex = j;
                    steps.Add(CreateStep(
                        array,
                        SortingOperation.SelectMinimum,
                        "Nove minimum",
                        $"Nasli jsme nove minimum {array[minIndex]} na indexu {minIndex}.",
                        comparisons,
                        swaps,
                        writes,
                        currentIndex: i,
                        targetIndex: minIndex));
                }
            }

            if (minIndex != i)
            {
                (array[i], array[minIndex]) = (array[minIndex], array[i]);
                swaps++;
                writes += 2;

                steps.Add(CreateStep(
                    array,
                    SortingOperation.Swap,
                    "Swap",
                    "Presouvame minimum {array[i]} na index {i}.",
                    comparisons,
                    swaps,
                    writes,
                    swappingIndices: new[] { i, minIndex },
                    currentIndex: i,
                    targetIndex: minIndex));
            }

            var isFinal = i == array.Length - 1;
            steps.Add(CreateStep(
                array,
                isFinal ? SortingOperation.Finished : SortingOperation.MarkSorted,
                isFinal ? "Hotovo" : "Umisteni minima",
                isFinal ? "Selection Sort dokonceno." : $"Prvek na indexu {i} je nyni na spravnem miste.",
                comparisons,
                swaps,
                writes,
                isMajorStep: true,
                groupTitle: isFinal ? "Finalni serazeni" : $"Po umisteni minima na index {i}",
                sortedIndices: BuildSortedIndices(i)));
        }

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
        IEnumerable<int>? sortedIndices = null,
        int? currentIndex = null,
        int? targetIndex = null)
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
            SortedIndices = sortedIndices?.ToList() ?? new List<int>(),
            CurrentIndex = currentIndex,
            TargetIndex = targetIndex
        };
    }

    private static IEnumerable<int> BuildSortedIndices(int lastSortedIndex)
    {
        if (lastSortedIndex < 0)
        {
            return Array.Empty<int>();
        }

        return Enumerable.Range(0, lastSortedIndex + 1);
    }
}
