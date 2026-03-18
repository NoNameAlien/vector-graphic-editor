using System.Collections.ObjectModel;
using VecEditor.Core.Geometry;

namespace VecEditor.ViewModel;

public sealed class EditorObjectsState : NotifyBase
{
    public ObservableCollection<PrimitiveObjectState> PrimitiveObjects { get; } = new();
}

public sealed class PrimitiveObjectState
{
    public string PrimitiveType { get; set; } = string.Empty;
    public string ToolType { get; set; } = string.Empty;
    public List<Point2> ObjectPoints { get; set; } = new();
}