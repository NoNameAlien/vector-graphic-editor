using System.Collections.ObjectModel;
using VecEditor.Core.Geometry;
using Avalonia;

namespace VecEditor.ViewModel;

public sealed class EditorObjectsState : NotifyBase
{
    public ObservableCollection<PrimitiveObjectState> PrimitiveObjects { get; set; } = new();

    public EditorObjectsState()
    {
        PrimitiveObjects = new ObservableCollection<PrimitiveObjectState>();
    }
    public EditorObjectsState(EditorObjectsState other)
    {
        this.PrimitiveObjects = new ObservableCollection<PrimitiveObjectState>();
        foreach (var obj in other.PrimitiveObjects)
        {
            this.PrimitiveObjects.Add(new PrimitiveObjectState(obj));
        }
    }
}

public sealed class PrimitiveObjectState : NotifyBase
{
    public PrimitiveObjectState(PrimitiveObjectState obj)
    {
        PrimitiveType = obj.PrimitiveType;
        ToolType  = obj.ToolType;
        ObjectPoints = new List<Point2>(obj.ObjectPoints);

        StrokeColor  = "Red";
        StrokeThickness = 2;

        _isSelected = false;
    }

    public PrimitiveObjectState(Enum SelectedPrimitive, Enum SelectedTool, List<Avalonia.Point> _tempPoints, string SelectedStrokeColor, double SelectedStrokeThickness)
    {
        PrimitiveType = SelectedPrimitive.ToString();
        ToolType = SelectedTool.ToString();
        ObjectPoints = _tempPoints
            .Take(2)
            .Select(p => new VecEditor.Core.Geometry.Point2(p.X, p.Y))
            .ToList();
        StrokeColor = SelectedStrokeColor;
        StrokeThickness = SelectedStrokeThickness;
    }

    public string PrimitiveType { get; set; } = string.Empty;
    public string ToolType { get; set; } = string.Empty;
    public List<Point2> ObjectPoints { get; set; } = new();

    public string StrokeColor { get; set; } = "Red";
    public double StrokeThickness { get; set; } = 2;

    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => Set(ref _isSelected, value);
    }

    public double BoundsX =>
        ObjectPoints.Count >= 2 ? Math.Min(ObjectPoints[0].X, ObjectPoints[1].X) : 0;

    public double BoundsY =>
        ObjectPoints.Count >= 2 ? Math.Min(ObjectPoints[0].Y, ObjectPoints[1].Y) : 0;

    public double BoundsWidth =>
        ObjectPoints.Count >= 2 ? Math.Abs(ObjectPoints[1].X - ObjectPoints[0].X) : 0;

    public double BoundsHeight =>
        ObjectPoints.Count >= 2 ? Math.Abs(ObjectPoints[1].Y - ObjectPoints[0].Y) : 0;
}


public class HistoryManager
{
    private List<EditorObjectsState> editorObjectsStates = new List<EditorObjectsState>();
    private int _index;
    public HistoryManager() {
        _index = -1;
        addState(new());
    }
    
    public bool CheckNotEmpty()
    {
        return _index >= 0; 
    }
    public EditorObjectsState getNextState()
    {
        if (_index < editorObjectsStates.Count - 1)
        {
            _index++;
        }
        return new(editorObjectsStates[_index]);
    }
    public EditorObjectsState getPrewState()
    {
        if (_index > 0)
        {
            _index--;
        }
        return new(editorObjectsStates[_index]);
    }
    public void addState(EditorObjectsState new_state)
    {
        _index++;
        editorObjectsStates.RemoveRange(_index, editorObjectsStates.Count - _index);
        editorObjectsStates.Add(new(new_state));
    }

    public void Clear()
    {
        editorObjectsStates.Clear();
        _index = -1;
        addState(new EditorObjectsState()); // Добавляем пустое состояние
    }

}