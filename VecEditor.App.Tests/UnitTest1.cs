
using Avalonia;
using System.Drawing;
using VecEditor.ViewModel;

namespace VecEditor.App.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test2()
        {
            var C1 = new MainViewModel();
            Avalonia.Point a = new Avalonia.Point(0, 0);
            Avalonia.Point b = new Avalonia.Point(1, 1);
            //C1.tmp_points.Add(a);
            //C1.tmp_points.Add(b);

            //Assert.Equal(b, C1.tmp_points[1]);
            //Assert.Equal(a, C1.tmp_points[0]);
            //Assert.Equal(2, C1.tmp_points.Count());

        }

        [Fact]
        public void Test3()
        {
            var C1 = new MainViewModel();
            Avalonia.Point A = new Avalonia.Point();
            Avalonia.Point B = new Avalonia.Point();
            //C1.Add_point(A);
            //Assert.Empty(C1.objects);
            //Assert.Single(C1.tmp_points);
            //C1.Add_point(B);
            //Assert.Empty(C1.tmp_points);
            //Assert.Single(C1.objects);
        }
    }
}