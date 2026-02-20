using System;
using System.Composition;
using Newtonsoft.Json;
using VecEditor.Core.Geometry;

namespace VecEditor.Core.Figures;

[Export(typeof(IFigure))]
[ExportMetadata("Name", nameof(LineFigure))]
public sealed class LineFigure : IFigure
{
    public Guid Id { get; } = Guid.NewGuid();
    public Point2 A { get; }
    public Point2 B { get; }

    [JsonIgnore]
    public RectD Bounds => RectD.FromTwoPoints(A, B);

    public LineFigure(Point2 a, Point2 b)
    {
        A = a;
        B = b;
    }

    public IFigure Clone() => throw new NotImplementedException();
}
