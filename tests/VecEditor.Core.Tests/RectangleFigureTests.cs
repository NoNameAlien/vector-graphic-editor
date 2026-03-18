using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;
using Xunit;

namespace VecEditor.Core.Tests;

public class RectangleFigureTests
{
    [Fact]
    public void Bounds_AreCalculatedCorrectly()
    {
        var rect = new RectangleFigure(new Point2(10, 20), new Point2(30, 50));

        Assert.Equal(10, rect.Bounds.X);
        Assert.Equal(20, rect.Bounds.Y);
        Assert.Equal(20, rect.Bounds.Width);
        Assert.Equal(30, rect.Bounds.Height);
    }
}