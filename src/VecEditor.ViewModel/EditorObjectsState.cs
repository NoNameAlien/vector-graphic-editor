using System.Collections.ObjectModel;
using VecEditor.Core.Geometry;

namespace VecEditor.ViewModel;

public sealed class EditorObjectsState : NotifyBase
{
    public ObservableCollection<PrimitiveObjectState> PrimitiveObjects { get; } = new();
}

public sealed class PrimitiveObjectState : NotifyBase
{
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