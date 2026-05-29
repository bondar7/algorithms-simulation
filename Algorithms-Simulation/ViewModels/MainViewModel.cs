using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Algorithms_Simulation.Algorithms;
using Algorithms_Simulation.Models;
using Algorithms_Simulation.Services;

namespace Algorithms_Simulation.ViewModels;

public class MainViewModel : ObservableObject
{
    private const int MaxArrayLength = 12;
    private readonly ArrayParser _arrayParser = new();
    private readonly RandomArrayGenerator _randomArrayGenerator = new();
    private int[] _inputArray = Array.Empty<int>();
    private ISortingAlgorithm? _selectedAlgorithm;
    private string _inputText = "5, 2, 9, 1, 4";
    private IReadOnlyList<SortingStep> _steps = Array.Empty<SortingStep>();
    private int _currentStepIndex;
    private SortingStep? _currentStep;
    private string _currentExplanation = string.Empty;
    private string _errorMessage = string.Empty;
    private int _currentHistoryIndex;

    public MainViewModel()
    {
        Algorithms = SortingAlgorithmRegistry.GetAlgorithms();
        ArrayItems = new ObservableCollection<ArrayItemViewModel>();
        HistoryRows = new ObservableCollection<HistoryRowViewModel>();
        LoadInitialArray();

        SelectedAlgorithm = Algorithms.FirstOrDefault();

        StartCommand = new RelayCommand(Start, CanStart);
        NextStepCommand = new RelayCommand(NextStep, CanMoveNext);
        PreviousStepCommand = new RelayCommand(PreviousStep, CanMovePrevious);
        ResetCommand = new RelayCommand(Reset);
        GenerateRandomCommand = new RelayCommand(GenerateRandomArray);
    }

    public IReadOnlyList<ISortingAlgorithm> Algorithms { get; }

    public ISortingAlgorithm? SelectedAlgorithm
    {
        get => _selectedAlgorithm;
        set
        {
            if (SetProperty(ref _selectedAlgorithm, value))
            {
                Reset();
                InvalidateCommands();
            }
        }
    }

    public string InputText
    {
        get => _inputText;
        set
        {
            if (SetProperty(ref _inputText, value))
            {
                InvalidateCommands();
            }
        }
    }

    public ObservableCollection<ArrayItemViewModel> ArrayItems { get; }

    public ObservableCollection<HistoryRowViewModel> HistoryRows { get; }

    public IReadOnlyList<SortingStep> Steps
    {
        get => _steps;
        private set => SetProperty(ref _steps, value);
    }

    public int CurrentStepIndex
    {
        get => _currentStepIndex;
        private set
        {
            if (SetProperty(ref _currentStepIndex, value))
            {
                OnPropertyChanged(nameof(CurrentStepDisplay));
            }
        }
    }

    public int CurrentStepDisplay => Steps.Count == 0 ? 0 : CurrentStepIndex + 1;

    public SortingStep? CurrentStep
    {
        get => _currentStep;
        private set => SetProperty(ref _currentStep, value);
    }

    public string CurrentExplanation
    {
        get => _currentExplanation;
        private set => SetProperty(ref _currentExplanation, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public int CurrentHistoryIndex
    {
        get => _currentHistoryIndex;
        private set => SetProperty(ref _currentHistoryIndex, value);
    }

    public ICommand StartCommand { get; }
    public ICommand NextStepCommand { get; }
    public ICommand PreviousStepCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand GenerateRandomCommand { get; }

    private void Start()
    {
        ErrorMessage = string.Empty;

        if (SelectedAlgorithm is null)
        {
            ErrorMessage = "Vyberte prosim algoritmus razeni.";
            return;
        }

        if (!_arrayParser.TryParse(InputText, MaxArrayLength, out var values, out var errorMessage))
        {
            ErrorMessage = errorMessage;
            return;
        }

        _inputArray = values.ToArray();
        Steps = SelectedAlgorithm.GenerateSteps(values);
        CurrentStepIndex = 0;
        SetCurrentStep(Steps.FirstOrDefault());
        UpdateHistoryRows();
        InvalidateCommands();
    }

    private void NextStep()
    {
        if (Steps.Count == 0 || CurrentStepIndex >= Steps.Count - 1)
        {
            return;
        }

        CurrentStepIndex++;
        SetCurrentStep(Steps[CurrentStepIndex]);
        UpdateHistoryRows();
        InvalidateCommands();
    }

    private void PreviousStep()
    {
        if (Steps.Count == 0 || CurrentStepIndex <= 0)
        {
            return;
        }

        CurrentStepIndex--;
        SetCurrentStep(Steps[CurrentStepIndex]);
        UpdateHistoryRows();
        InvalidateCommands();
    }

    private void Reset()
    {
        ErrorMessage = string.Empty;
        Steps = Array.Empty<SortingStep>();
        CurrentStepIndex = 0;
        CurrentStep = null;
        CurrentExplanation = string.Empty;
        CurrentHistoryIndex = 0;
        HistoryRows.Clear();

        LoadInitialArray();
        InvalidateCommands();
    }

    private bool CanStart() => SelectedAlgorithm is not null;

    private bool CanMoveNext() => Steps.Count > 0 && CurrentStepIndex < Steps.Count - 1;

    private bool CanMovePrevious() => Steps.Count > 0 && CurrentStepIndex > 0;

    private void LoadInitialArray()
    {
        if (_arrayParser.TryParse(InputText, MaxArrayLength, out var values, out _))
        {
            _inputArray = values.ToArray();
        }
        else
        {
            _inputArray = new[] { 5, 2, 9, 1, 4 };
        }

        UpdateArrayItems(_inputArray, null);
    }

    private void GenerateRandomArray()
    {
        ErrorMessage = string.Empty;
        var values = _randomArrayGenerator.Generate(8, 1, 99);
        InputText = string.Join(", ", values);
        Reset();
    }

    private void SetCurrentStep(SortingStep? step)
    {
        CurrentStep = step;
        CurrentExplanation = step?.Description ?? string.Empty;

        if (step is null)
        {
            UpdateArrayItems(_inputArray, null);
            return;
        }

        UpdateArrayItems(step.ArrayState, step);
    }

    private void UpdateArrayItems(int[] array, SortingStep? step)
    {
        ArrayItems.Clear();
        foreach (var item in BuildArrayItems(array, step))
        {
            ArrayItems.Add(item);
        }
    }

    private void UpdateHistoryRows()
    {
        HistoryRows.Clear();

        if (Steps.Count == 0)
        {
            CurrentHistoryIndex = 0;
            return;
        }

        var activeIndex = -1;

        for (var i = 0; i <= CurrentStepIndex && i < Steps.Count; i++)
        {
            var step = Steps[i];
            if (!step.IsMajorStep)
            {
                continue;
            }

            var title = string.IsNullOrWhiteSpace(step.GroupTitle) ? step.Title : step.GroupTitle;
            var items = new ObservableCollection<ArrayItemViewModel>(BuildArrayItems(step.ArrayState, step));
            HistoryRows.Add(new HistoryRowViewModel(i, title, items));
            activeIndex = HistoryRows.Count - 1;
        }

        CurrentHistoryIndex = Math.Max(activeIndex, 0);

        for (var i = 0; i < HistoryRows.Count; i++)
        {
            HistoryRows[i].IsActive = i == CurrentHistoryIndex;
        }
    }

    private static IEnumerable<ArrayItemViewModel> BuildArrayItems(int[] array, SortingStep? step)
    {
        var comparing = step?.ComparingIndices ?? new List<int>();
        var swapping = step?.SwappingIndices ?? new List<int>();
        var moving = step?.MovingIndices ?? new List<int>();
        var sorted = step?.SortedIndices ?? new List<int>();
        var pivot = step?.PivotIndex;
        var current = step?.CurrentIndex;
        var target = step?.TargetIndex;

        for (var i = 0; i < array.Length; i++)
        {
            yield return new ArrayItemViewModel
            {
                Index = i,
                Value = array[i],
                IsCompared = comparing.Contains(i),
                IsSwapped = swapping.Contains(i),
                IsMoved = moving.Contains(i),
                IsSorted = sorted.Contains(i),
                IsPivot = pivot == i,
                IsCurrent = current == i,
                IsTarget = target == i
            };
        }
    }

    private static void InvalidateCommands()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
