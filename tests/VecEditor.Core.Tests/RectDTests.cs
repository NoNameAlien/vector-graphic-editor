using VecEditor.Core.Geometry;
using Xunit;

namespace VecEditor.Core.Tests;

public class RectDTests
{
    [Fact]
    public void FromTwoPoints_WorksWithReverseOrder()
    {
        var rect = RectD.FromTwoPoints(new Point2(30, 50), new Point2(10, 20));

        Assert.Equal(10, rect.X);
        Assert.Equal(20, rect.Y);
        Assert.Equal(20, rect.Width);
        Assert.Equal(30, rect.Height);
    }
}