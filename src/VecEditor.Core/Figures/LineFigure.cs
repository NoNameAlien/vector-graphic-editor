using System;
using VecEditor.Core.Geometry;

namespace VecEditor.Core.Figures;

public sealed class LineFigure : IFigure
{
    public Guid Id { get; } = Guid.NewGuid();
    public Point2 A { get; }
    public Point2 B { get; }

    public RectD Bounds => RectD.FromTwoPoints(A, B);

    public LineFigure(Point2 a, Point2 b)
    {
        A = a;
        B = b;
    }
}