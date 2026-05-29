namespace Algorithms_Simulation.ViewModels;

public class ArrayItemViewModel : ObservableObject
{
    private int _value;
    private int _index;
    private bool _isCompared;
    private bool _isSwapped;
    private bool _isMoved;
    private bool _isSorted;
    private bool _isPivot;
    private bool _isCurrent;
    private bool _isTarget;

    public int Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public int Index
    {
        get => _index;
        set => SetProperty(ref _index, value);
    }

    public bool IsCompared
    {
        get => _isCompared;
        set => SetProperty(ref _isCompared, value);
    }

    public bool IsSwapped
    {
        get => _isSwapped;
        set => SetProperty(ref _isSwapped, value);
    }

    public bool IsMoved
    {
        get => _isMoved;
        set => SetProperty(ref _isMoved, value);
    }

    public bool IsSorted
    {
        get => _isSorted;
        set => SetProperty(ref _isSorted, value);
    }

    public bool IsPivot
    {
        get => _isPivot;
        set => SetProperty(ref _isPivot, value);
    }

    public bool IsCurrent
    {
        get => _isCurrent;
        set => SetProperty(ref _isCurrent, value);
    }

    public bool IsTarget
    {
        get => _isTarget;
        set => SetProperty(ref _isTarget, value);
    }
}
