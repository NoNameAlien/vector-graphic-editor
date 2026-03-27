using Avalonia;
using System.Drawing;
using VecEditor.App.ViewModels;
using VecEditor.ViewModel;
using static VecEditor.ViewModel.MainViewModel;
using static VecEditor.App.ViewModels.MainWindowViewModel;
using System.Reflection;
using System.Security.Cryptography;

namespace VecEditor.App.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test2()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Avalonia.Point a = new Avalonia.Point(0, 0);
            c1.AddPoint(a);
            Assert.True(c1.IsDrawing);
            Avalonia.Point b = new Avalonia.Point(1, 0);
            c1.AddPoint(b);
            Assert.False(c1.IsDrawing);
            c1.SelectedPrimitive = MainWindowViewModel.PrimitiveType.None;
            c1.AddPoint(a);
            Assert.Null(c1.PreviewStartPoint);
        }

        [Fact]
        public void Test3_update_preview()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Avalonia.Point A = new Avalonia.Point();
            Avalonia.Point B = new Avalonia.Point();
        }
        [Fact]
        public void Test4_update_preview()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Avalonia.Point a = new Avalonia.Point(0, 0);
            Avalonia.Point b = new Avalonia.Point(1, 0);
            c1.UpdatePreview(a);
            Assert.False(c1.IsDrawing);
            c1.AddPoint(a);
            c1.UpdatePreview(b);
            Assert.Equal(b, c1.PreviewEndPoint);
        }

        [Fact]
        public void Test5_selected_object_at()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Avalonia.Point a = new Avalonia.Point(0, 0);
            Avalonia.Point h = new Avalonia.Point(0.5, 0.5);
            Avalonia.Point b = new Avalonia.Point(1, 1);
            Avalonia.Point c = new Avalonia.Point(2, 3);
            c1.SelectedPrimitive = PrimitiveType.Rectangle;
            c1.AddPoint(a);
            c1.AddPoint(b);
            c1.SelectObjectAt(c);
            Assert.Null(c1.SelectedObject);
            Avalonia.Point d = new Avalonia.Point(2, 0);
            Avalonia.Point e = new Avalonia.Point(1, 0);
            Avalonia.Point g = new Avalonia.Point(50, 100);
            c1.SelectedPrimitive = PrimitiveType.Line;
            c1.AddPoint(d);
            c1.AddPoint(a);
            c1.SelectObjectAt(e);
            Assert.NotNull(c1.SelectedObject);
            c1.SelectObjectAt(g);
            Assert.Null(c1.SelectedObject);
            c1.SelectObjectAt(h);
            Assert.NotNull(c1.SelectedObject);
        }
        [Fact]
        public void Test6_delete_object_at()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Avalonia.Point a = new Avalonia.Point(0, 0);
            Avalonia.Point h = new Avalonia.Point(0.5, 0.5);
            Avalonia.Point b = new Avalonia.Point(1, 1);
            c1.SelectedPrimitive = PrimitiveType.Rectangle;
            c1.AddPoint(a);
            c1.AddPoint(b);
            Assert.NotEmpty(c1.PrimitiveObjects);
            c1.DeleteObjectAt(h);
            Assert.Empty(c1.PrimitiveObjects);
        }

        [Fact]
        public void Test7_is_point_inside_square()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Avalonia.Point a = new Avalonia.Point(1, 1);
            Avalonia.Point b = new Avalonia.Point(0, 0);
            c1.SelectedPrimitive = PrimitiveType.Rectangle;
            c1.AddPoint(a);
            c1.AddPoint(b);
            c1.SelectObjectAt(new Avalonia.Point(0.5, 0.5));
            Assert.NotNull(c1.SelectedObject);
        }
        [Fact]
        public void Test8_undo_redo()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            c1.Undo();
            c1.Redo();

            Avalonia.Point a = new Avalonia.Point(1, 1);
            Avalonia.Point b = new Avalonia.Point(0, 0);
            c1.SelectedPrimitive = PrimitiveType.Rectangle;
            c1.AddPoint(a);
            c1.AddPoint(b);
            c1.AddPoint(a);
            c1.AddPoint(b);
            Assert.NotEmpty(c1.PrimitiveObjects);
            c1.Undo();
            Assert.Single(c1.PrimitiveObjects);
            c1.Redo();
            Assert.NotEmpty(c1.PrimitiveObjects);
        }

        [Fact]
        public void Test9_MVM()
        {
            var b1 = new VecEditor.ViewModel.MainViewModel();
            Assert.Empty(b1.Figures);
            Avalonia.Point a = new Avalonia.Point(1, 1);
            Avalonia.Point b = new Avalonia.Point(0, 0);
            b1.AddDemoLine();
        }
        [Fact]
        public void Test10_Editor_State()
        {
            var b1 = new VecEditor.ViewModel.EditorState();
            Assert.Equal("Pointer", b1.SelectedTool);
            b1.SelectedTool = "Eraser";
            Assert.Equal("Eraser", b1.SelectedTool);
            Assert.Equal("Line", b1.SelectedPrimitive);
            b1.SelectedPrimitive = "Rectangle";
            Assert.Equal("Rectangle", b1.SelectedPrimitive);
            b1.SelectedPrimitive = "Rectangle";
        }
        [Fact]
        public void Test11_Primitive_Object_State()
        {
            List<Avalonia.Point> list = new List<Avalonia.Point>();
            Avalonia.Point a = new Avalonia.Point(1, 1);
            Avalonia.Point b = new Avalonia.Point(0, 0);
            list.Add(a);
            list.Add(b);
            string red = "Red";
            var b1 = new VecEditor.ViewModel.PrimitiveObjectState(PrimitiveType.Rectangle, ToolType.None, list, red, 2.0);

            Assert.Equal(0, b1.BoundsX);
            Assert.Equal(0, b1.BoundsY);
            Assert.Equal(1, b1.BoundsWidth);
            Assert.Equal(1, b1.BoundsHeight);
            Assert.False(b1.IsSelected);
        }
        [Fact]
        public void Test12_PropertiesPanel()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Assert.Equal("2 px", c1.SelectedStrokeThicknessDisplay);
            c1.SelectedStrokeThicknessDisplay = "";
            c1.SelectedStrokeThicknessDisplay = "4 px";
            Assert.Equal("4 px", c1.SelectedStrokeThicknessDisplay);
            c1.SelectedStrokeThicknessDisplay = "-#@:";
            Assert.Equal("4 px", c1.SelectedStrokeThicknessDisplay);
            c1.SelectedObject = null;
            Assert.Equal("Line", c1.CurrentFigureTypeText);
            Avalonia.Point a = new Avalonia.Point(1, 1);
            Avalonia.Point b = new Avalonia.Point(0, 0);
            c1.AddPoint(a);
            c1.AddPoint(b);
            c1.SelectedObject = c1.PrimitiveObjects[0];
            string str = c1.CurrentFigureTypeText;
            c1.SelectedPrimitive = PrimitiveType.None;
            Assert.Equal("Line", c1.CurrentFigureTypeText);
            c1.SelectedTool = ToolType.None;
            Assert.Equal("Line", c1.CurrentFigureTypeText);
        }
        [Fact]
        public void Test13()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            string str = c1.CurrentXText;
            string str1 = c1.CurrentYText;
            string str2 = c1.CurrentHeightText;
            string str3 = c1.CurrentWidthText;


        }
    }
}