using System.Collections.Generic;
using System.Linq;
using Algorithms_Simulation.Models;

namespace Algorithms_Simulation.Algorithms;

public class QuickSortAlgorithm : ISortingAlgorithm
{
    public string Id => "quick";
    public string Name => "Quick Sort";
    public string Description => "Rekurzivne rozdeluje pole podle pivotu na mensi casti.";
    public string TimeComplexity => "Nejlepsi: O(n log n), Prumer: O(n log n), Nejhorsi: O(n^2)";
    public string SpaceComplexity => "O(log n)";
    public string Pseudocode => "quickSort(a, low, high)\n  if low < high\n    p = partition(a, low, high)\n    quickSort(a, low, p-1)\n    quickSort(a, p+1, high)";

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
        var fixedIndices = new HashSet<int>();

        steps.Add(CreateStep(
            array,
            SortingOperation.None,
            "Start",
            "Zacina Quick Sort.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Pocatecni stav"));

        if (array.Length > 1)
        {
            QuickSort(array, 0, array.Length - 1, steps, fixedIndices, ref comparisons, ref swaps, ref writes);
        }
        else if (array.Length == 1)
        {
            fixedIndices.Add(0);
        }

        steps.Add(CreateStep(
            array,
            SortingOperation.Finished,
            "Hotovo",
            "Quick Sort dokonceno.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Finalni serazeni",
            sortedIndices: Enumerable.Range(0, array.Length)));

        return steps;
    }

    private static void QuickSort(
        int[] array,
        int low,
        int high,
        List<SortingStep> steps,
        HashSet<int> fixedIndices,
        ref int comparisons,
        ref int swaps,
        ref int writes)
    {
        if (low > high)
        {
            return;
        }

        if (low == high)
        {
            fixedIndices.Add(low);
            return;
        }

        var pivotIndex = Partition(array, low, high, steps, fixedIndices, ref comparisons, ref swaps, ref writes);
        fixedIndices.Add(pivotIndex);

        steps.Add(CreateStep(
            array,
            SortingOperation.Partition,
            "Rozdeleni dokonceno",
            $"Pivot je na finalni pozici {pivotIndex} v rozsahu {low}–{high}.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: $"Pivot na finalni pozici {pivotIndex}",
            pivotIndex: pivotIndex,
            sortedIndices: fixedIndices));

        QuickSort(array, low, pivotIndex - 1, steps, fixedIndices, ref comparisons, ref swaps, ref writes);
        QuickSort(array, pivotIndex + 1, high, steps, fixedIndices, ref comparisons, ref swaps, ref writes);
    }

    private static int Partition(
        int[] array,
        int low,
        int high,
        List<SortingStep> steps,
        HashSet<int> fixedIndices,
        ref int comparisons,
        ref int swaps,
        ref int writes)
    {
        var pivot = array[high];
        var i = low - 1;

        steps.Add(CreateStep(
            array,
            SortingOperation.ChoosePivot,
            "Vyber pivotu",
            $"Pivot je {pivot} na indexu {high}.",
            comparisons,
            swaps,
            writes,
            pivotIndex: high,
            currentIndex: low,
            targetIndex: low,
            sortedIndices: fixedIndices));

        for (var j = low; j < high; j++)
        {
            comparisons++;
            steps.Add(CreateStep(
                array,
                SortingOperation.Compare,
                "Porovnani",
                $"Porovnavame {array[j]} s pivotem {pivot}.",
                comparisons,
                swaps,
                writes,
                comparingIndices: new[] { j, high },
                pivotIndex: high,
                currentIndex: j,
                targetIndex: i + 1,
                sortedIndices: fixedIndices));

            if (array[j] <= pivot)
            {
                i++;
                if (i != j)
                {
                    (array[i], array[j]) = (array[j], array[i]);
                    swaps++;
                    writes += 2;

                    steps.Add(CreateStep(
                        array,
                        SortingOperation.Swap,
                        "Swap",
                        "Vymenujeme {array[i]} a {array[j]}.",
                        comparisons,
                        swaps,
                        writes,
                        swappingIndices: new[] { i, j },
                        pivotIndex: high,
                        currentIndex: j,
                        targetIndex: i,
                        sortedIndices: fixedIndices));
                }
            }
        }

        if (i + 1 != high)
        {
            (array[i + 1], array[high]) = (array[high], array[i + 1]);
            swaps++;
            writes += 2;

            steps.Add(CreateStep(
                array,
                SortingOperation.Swap,
                "Swap",
                $"Pivot {pivot} presouvame na index {i + 1}.",
                comparisons,
                swaps,
                writes,
                swappingIndices: new[] { i + 1, high },
                pivotIndex: i + 1,
                currentIndex: i + 1,
                targetIndex: i + 1,
                sortedIndices: fixedIndices));
        }

        return i + 1;
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
        int? pivotIndex = null,
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
            PivotIndex = pivotIndex,
            CurrentIndex = currentIndex,
            TargetIndex = targetIndex
        };
    }
}
