using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;

namespace VecEditor.Core.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_Line1()
    {
        // Arrange
        var pointA = new Point2(1, 2);
        var pointB = new Point2(3, 4);

        // Act
        var line = new LineFigure(pointA, pointB);

        // Assert
        Assert.Equal(pointA, line.A);
        Assert.Equal(pointB, line.B);
    }
}
