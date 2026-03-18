using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;
using Xunit;

namespace VecEditor.Core.Tests;

public class EllipseFigureTests
{
    [Fact]
    public void Bounds_AreCalculatedCorrectly()
    {
        var ellipse = new EllipseFigure(new Point2(0, 0), new Point2(20, 10));

        Assert.Equal(0, ellipse.Bounds.X);
        Assert.Equal(0, ellipse.Bounds.Y);
        Assert.Equal(20, ellipse.Bounds.Width);
        Assert.Equal(10, ellipse.Bounds.Height);
    }

    [Fact]
    public void Clone_ReturnsNewEllipseWithSamePoints()
    {
        var ellipse = new EllipseFigure(new Point2(1, 2), new Point2(3, 4));

        var clone = ellipse.Clone();

        Assert.NotSame(ellipse, clone);
        Assert.IsType<EllipseFigure>(clone);
    }
}