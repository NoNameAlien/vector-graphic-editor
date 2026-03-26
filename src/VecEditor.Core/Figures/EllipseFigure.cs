using System;
using VecEditor.Core.Geometry;

namespace VecEditor.Core.Figures;

public sealed class EllipseFigure : IFigure
{
    public Guid Id { get; } = Guid.NewGuid();

    public Point2 A { get; set; }
    public Point2 B { get; set; }

    public RectD Bounds => RectD.FromTwoPoints(A, B);

    public EllipseFigure(Point2 a, Point2 b)
    {
        A = a;
        B = b;
    }



    public void Move(double dx, double dy)
    {
        Point2 tmp = new Point2(A.X + dx, A.Y + dy);
        A = tmp;
        tmp = new Point2(B.X + dx, B.Y + dy);
        B = tmp;
    }

    public bool HitTest(Point2 point, double tolerance = 1.0)
    {
        if (point.X < A.X || point.X > B.X || point.Y < A.Y || point.Y > B.Y)
        {
            return false;
        }
        return true;
    }
    public IFigure Clone() => new EllipseFigure(A, B);
}