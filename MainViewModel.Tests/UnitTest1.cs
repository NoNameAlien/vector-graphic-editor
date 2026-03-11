using System.Security.Cryptography;
using VecEditor.ViewModel;
using static VecEditor.ViewModel.MainViewModel;
namespace MainViewModel.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1_Instruments_init()
        {
            var c1 = new VecEditor.ViewModel.MainViewModel();
            Assert.False(c1.IsPenActive);
            Assert.False(c1.IsPencilActive);
            Assert.False(c1.IsEraserActive);
            Assert.False(c1.IsBrushActive);
            Assert.False(c1.IsToolSelected);

            c1.SelectedTool = ToolType.Brush;
            Assert.Equal(ToolType.Brush, c1.SelectedTool);

            
        }
        [Fact]
        public void Test2_Primitive_init()
        {
            var c1 = new VecEditor.ViewModel.MainViewModel();
            Assert.False(c1.IsEllipseActive);
            Assert.False(c1.IsLineActive);
            Assert.False(c1.IsRectangleActive);
            Assert.False(c1.IsCircleActive);
            Assert.False(c1.IsArrowActive);
            Assert.False(c1.IsTriangleActive);


        }
        [Fact]
        public void Test2_Primitive_object()
        {
            var c1 = new VecEditor.ViewModel.MainViewModel();
            var prim_obj = new PrimitiveObject(PrimitiveType.Triangle, new Avalonia.Point(0,0));
            Assert.Empty(c1.primitiveObjects);
            c1.SelectedPrimitive = PrimitiveType.Triangle;
            c1.add_object(new Avalonia.Point(0, 0));
            Assert.Single(c1.primitiveObjects);
            //var c12 = c1.primitiveObjects[0];
            
        }
        [Fact]
        public void Test3_init()
        {
            var c1 = new VecEditor.ViewModel.MainViewModel();
            //var prim_obj = new PrimitiveObject(PrimitiveType.Triangle, new Avalonia.Point(0, 0));
            //Assert.Empty(c1.primitiveObjects);
            //c1.SelectedPrimitive = PrimitiveType.Triangle;
            //c1.add_object(new Avalonia.Point(0, 0));
            //Assert.Single(c1.primitiveObjects);
            //var c12 = c1.primitiveObjects[0];

        }
    }
}