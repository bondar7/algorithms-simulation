using System.Collections.Generic;
using System.Linq;
using Algorithms_Simulation.Models;

namespace Algorithms_Simulation.Algorithms;

public class InsertionSortAlgorithm : ISortingAlgorithm
{
    public string Id => "insertion";
    public string Name => "Insertion Sort";
    public string Description => "Postupne vklada prvky do serazene casti pole zleva.";
    public string TimeComplexity => "Nejlepsi: O(n), Prumer: O(n^2), Nejhorsi: O(n^2)";
    public string SpaceComplexity => "O(1)";
    public string Pseudocode => "for i = 1 to n-1\n  key = a[i]\n  j = i-1\n  while j >= 0 and a[j] > key\n    a[j+1] = a[j]\n    j = j-1\n  a[j+1] = key";

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
            "Zacina Insertion Sort.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Pocatecni stav"));

        for (var i = 1; i < array.Length; i++)
        {
            var key = array[i];
            var j = i - 1;

            steps.Add(CreateStep(
                array,
                SortingOperation.None,
                "Vyber klice",
                $"Vkladame prvek {key} z indexu {i}.",
                comparisons,
                swaps,
                writes,
                currentIndex: i,
                sortedIndices: BuildSortedIndices(i - 1)));

            while (j >= 0)
            {
                comparisons++;
                steps.Add(CreateStep(
                    array,
                    SortingOperation.Compare,
                    "Porovnani",
                    $"Porovnani {key} s {array[j]}.",
                    comparisons,
                    swaps,
                    writes,
                    comparingIndices: new[] { j, j + 1 },
                    currentIndex: i,
                    sortedIndices: BuildSortedIndices(i - 1)));

                if (array[j] <= key)
                {
                    break;
                }

                array[j + 1] = array[j];
                writes++;

                steps.Add(CreateStep(
                    array,
                    SortingOperation.Move,
                    "Posun",
                    $"Posouvame {array[j + 1]} doprava.",
                    comparisons,
                    swaps,
                    writes,
                    movingIndices: new[] { j, j + 1 },
                    currentIndex: i,
                    sortedIndices: BuildSortedIndices(i - 1)));

                j--;
            }

            var insertIndex = j + 1;
            array[insertIndex] = key;
            writes++;

            steps.Add(CreateStep(
                array,
                SortingOperation.Insert,
                "Vlozeni",
                $"Vkladame {key} na index {insertIndex}.",
                comparisons,
                swaps,
                writes,
                currentIndex: i,
                targetIndex: insertIndex,
                sortedIndices: BuildSortedIndices(i)));

            var isFinal = i == array.Length - 1;
            steps.Add(CreateStep(
                array,
                isFinal ? SortingOperation.Finished : SortingOperation.MarkSorted,
                isFinal ? "Hotovo" : "Serazena cast",
                isFinal ? "Insertion Sort dokonceno." : $"Serazena cast je 0 az {i}.",
                comparisons,
                swaps,
                writes,
                isMajorStep: true,
                groupTitle: isFinal ? "Finalni serazeni" : $"Po vlozeni prvku z indexu {i}",
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
        IEnumerable<int>? movingIndices = null,
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
            MovingIndices = movingIndices?.ToList() ?? new List<int>(),
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
