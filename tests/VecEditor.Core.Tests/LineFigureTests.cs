using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;
using Xunit;

namespace VecEditor.Core.Tests;

public class LineFigureTests
{
    [Fact]
    public void Clone_ReturnsNewObjectWithSamePoints()
    {
        var line = new LineFigure(new Point2(1, 2), new Point2(3, 4));

        var clone = line.Clone();

        Assert.NotSame(line, clone);
        Assert.IsType<LineFigure>(clone);

        var clonedLine = (LineFigure)clone;
        Assert.Equal(line.A, clonedLine.A);
        Assert.Equal(line.B, clonedLine.B);
    }
}