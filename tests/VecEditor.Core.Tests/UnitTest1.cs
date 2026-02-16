using VecEditor.Core.Geometry;
using Xunit;

public class RectDTests
{
    [Fact]
    public void FromTwoPoints_CreatesPositiveWidthHeight()
    {
        var r = RectD.FromTwoPoints(new Point2(10, 20), new Point2(5, 15));
        Assert.Equal(5, r.X);
        Assert.Equal(15, r.Y);
        Assert.Equal(5, r.Width);
        Assert.Equal(5, r.Height);
    }
}