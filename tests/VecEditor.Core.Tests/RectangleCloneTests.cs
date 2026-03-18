using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;
using Xunit;

namespace VecEditor.Core.Tests;

public class RectangleCloneTests
{
    [Fact]
    public void Clone_ReturnsNewRectangleWithSamePoints()
    {
        var rectangle = new RectangleFigure(new Point2(10, 20), new Point2(30, 50));

        var clone = rectangle.Clone();

        Assert.NotSame(rectangle, clone);
        Assert.IsType<RectangleFigure>(clone);

        var clonedRectangle = (RectangleFigure)clone;
        Assert.Equal(rectangle.A, clonedRectangle.A);
        Assert.Equal(rectangle.B, clonedRectangle.B);
    }
}