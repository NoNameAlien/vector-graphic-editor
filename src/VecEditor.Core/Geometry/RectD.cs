using System;

namespace VecEditor.Core.Geometry;

public readonly record struct RectD(double X, double Y, double Width, double Height)
{
    public static RectD FromTwoPoints(Point2 a, Point2 b)
    {
        var x1 = Math.Min(a.X, b.X);
        var y1 = Math.Min(a.Y, b.Y);
        var x2 = Math.Max(a.X, b.X);
        var y2 = Math.Max(a.Y, b.Y);
        return new RectD(x1, y1, x2 - x1, y2 - y1);
    }
}