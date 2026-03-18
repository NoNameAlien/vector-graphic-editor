using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;
using Xunit;

namespace VecEditor.Core.Tests;

public class LineBoundsTests
{
    [Fact]
    public void Bounds_AreCalculatedCorrectly()
    {
        var line = new LineFigure(new Point2(5, 10), new Point2(25, 30));

        Assert.Equal(5, line.Bounds.X);
        Assert.Equal(10, line.Bounds.Y);
        Assert.Equal(20, line.Bounds.Width);
        Assert.Equal(20, line.Bounds.Height);
    }
}