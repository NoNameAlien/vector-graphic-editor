using System.Collections.ObjectModel;
using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;

namespace VecEditor.ViewModel;

public sealed class MainViewModel : NotifyBase
{
    public ObservableCollection<IFigure> Figures { get; } = new();

    public void AddDemoLine()
        => Figures.Add(new LineFigure(new Point2(10, 10), new Point2(200, 120)));
}