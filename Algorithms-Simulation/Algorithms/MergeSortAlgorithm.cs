using System.Collections.Generic;
using System.Linq;
using Algorithms_Simulation.Models;

namespace Algorithms_Simulation.Algorithms;

public class MergeSortAlgorithm : ISortingAlgorithm
{
    public string Id => "merge";
    public string Name => "Merge Sort";
    public string Description => "Rekurzivne deli pole na mensi casti a nasledne je slucuje do serazeneho poradi.";
    public string TimeComplexity => "Nejlepsi: O(n log n), Prumer: O(n log n), Nejhorsi: O(n log n)";
    public string SpaceComplexity => "O(n)";
    public string Pseudocode => "mergeSort(a, left, right)\n  if left >= right return\n  mid = (left+right)/2\n  mergeSort(a, left, mid)\n  mergeSort(a, mid+1, right)\n  merge(a, left, mid, right)";

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
        var mergedIndices = new HashSet<int>();

        steps.Add(CreateStep(
            array,
            SortingOperation.None,
            "Start",
            "Zacina Merge Sort.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Pocatecni stav"));

        if (array.Length > 1)
        {
            MergeSort(array, 0, array.Length - 1, steps, mergedIndices, ref comparisons, ref writes);
        }
        else if (array.Length == 1)
        {
            mergedIndices.Add(0);
        }

        steps.Add(CreateStep(
            array,
            SortingOperation.Finished,
            "Hotovo",
            "Merge Sort dokonceno.",
            comparisons,
            swaps,
            writes,
            isMajorStep: true,
            groupTitle: "Finalni serazeni",
            sortedIndices: mergedIndices));

        return steps;
    }

    private static void MergeSort(
        int[] array,
        int left,
        int right,
        List<SortingStep> steps,
        HashSet<int> mergedIndices,
        ref int comparisons,
        ref int writes)
    {
        if (left >= right)
        {
            mergedIndices.Add(left);
            return;
        }

        var mid = (left + right) / 2;
        steps.Add(CreateStep(
            array,
            SortingOperation.Merge,
            "Rozdeleni",
            $"Zpracovavame rozsah {left}–{right}.",
            comparisons,
            0,
            writes,
            currentIndex: left,
            targetIndex: right,
            sortedIndices: mergedIndices));

        MergeSort(array, left, mid, steps, mergedIndices, ref comparisons, ref writes);
        MergeSort(array, mid + 1, right, steps, mergedIndices, ref comparisons, ref writes);
        Merge(array, left, mid, right, steps, mergedIndices, ref comparisons, ref writes);
    }

    private static void Merge(
        int[] array,
        int left,
        int mid,
        int right,
        List<SortingStep> steps,
        HashSet<int> mergedIndices,
        ref int comparisons,
        ref int writes)
    {
        var leftArray = array[left..(mid + 1)];
        var rightArray = array[(mid + 1)..(right + 1)];
        var i = 0;
        var j = 0;
        var k = left;

        while (i < leftArray.Length && j < rightArray.Length)
        {
            comparisons++;
            steps.Add(CreateStep(
                array,
                SortingOperation.Merge,
                "Porovnani",
                $"Porovnani {leftArray[i]} a {rightArray[j]}.",
                comparisons,
                0,
                writes,
                comparingIndices: new[] { left + i, mid + 1 + j },
                currentIndex: k,
                sortedIndices: mergedIndices));

            if (leftArray[i] <= rightArray[j])
            {
                array[k] = leftArray[i];
                i++;
            }
            else
            {
                array[k] = rightArray[j];
                j++;
            }

            writes++;
            steps.Add(CreateStep(
                array,
                SortingOperation.Merge,
                "Zapis",
                $"Zapisujeme hodnotu na index {k}.",
                comparisons,
                0,
                writes,
                movingIndices: new[] { k },
                currentIndex: k,
                targetIndex: k,
                sortedIndices: mergedIndices));

            k++;
        }

        while (i < leftArray.Length)
        {
            array[k] = leftArray[i];
            writes++;
            steps.Add(CreateStep(
                array,
                SortingOperation.Merge,
                "Zapis",
                $"Zapisujeme hodnotu na index {k}.",
                comparisons,
                0,
                writes,
                movingIndices: new[] { k },
                currentIndex: k,
                targetIndex: k,
                sortedIndices: mergedIndices));
            i++;
            k++;
        }

        while (j < rightArray.Length)
        {
            array[k] = rightArray[j];
            writes++;
            steps.Add(CreateStep(
                array,
                SortingOperation.Merge,
                "Zapis",
                $"Zapisujeme hodnotu na index {k}.",
                comparisons,
                0,
                writes,
                movingIndices: new[] { k },
                currentIndex: k,
                targetIndex: k,
                sortedIndices: mergedIndices));
            j++;
            k++;
        }

        for (var index = left; index <= right; index++)
        {
            mergedIndices.Add(index);
        }

        steps.Add(CreateStep(
            array,
            SortingOperation.Merge,
            "Slouceni dokonceno",
            $"Rozsah {left}–{right} je nyni serazeny po slouceni.",
            comparisons,
            0,
            writes,
            isMajorStep: true,
            groupTitle: $"Po slouceni rozsahu {left}–{right}",
            sortedIndices: mergedIndices));
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
}
