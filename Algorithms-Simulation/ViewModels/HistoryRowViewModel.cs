using System.Collections.ObjectModel;

namespace Algorithms_Simulation.ViewModels;

public class HistoryRowViewModel : ObservableObject
{
    private bool _isActive;

    public HistoryRowViewModel(int stepIndex, string title, ObservableCollection<ArrayItemViewModel> items)
    {
        StepIndex = stepIndex;
        Title = title;
        Items = items;
    }

    public int StepIndex { get; }

    public string Title { get; }

    public ObservableCollection<ArrayItemViewModel> Items { get; }

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
}
