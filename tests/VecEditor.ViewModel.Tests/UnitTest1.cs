using System.Security.Cryptography;
using VecEditor.ViewModel;
using VecEditor.App.ViewModels;
using static VecEditor.App.ViewModels.MainWindowViewModel;
using static VecEditor.ViewModel.MainViewModel;
namespace VecEditor.ViewModel.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1_Instruments_init()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Assert.False(c1.IsPenActive);
            Assert.False(c1.IsPencilActive);
            Assert.False(c1.IsEraserActive);
            Assert.False(c1.IsBrushActive);
            Assert.False(c1.IsToolSelected);
            Assert.False(c1.IsPointerActive);


            c1.SelectedTool = ToolType.Brush;
            Assert.Equal(ToolType.Brush, c1.SelectedTool);


        }
        [Fact]
        public void Test2_Primitive_init()
        {
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            Assert.False(c1.IsEllipseActive);
            //Assert.False(c1.IsLineActive);
            Assert.False(c1.IsRectangleActive);
            Assert.False(c1.IsCircleActive);
            Assert.False(c1.IsArrowActive);
            Assert.False(c1.IsTriangleActive);
            Assert.True(c1.IsLineActive);
            Assert.False(c1.HasSelectedObject);

        }

        [Fact]
        public void Test4_SelectedTool_RaisesPropertyChanged()
        {
            // Arrange
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            var propertyChangedRaised = false;

            c1.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(c1.SelectedTool))
                    propertyChangedRaised = true;
            };

            // Act
            c1.SelectedTool = ToolType.Pencil;

            // Assert
            Assert.True(propertyChangedRaised);
            Assert.Equal(ToolType.Pencil, c1.SelectedTool);
        }
        [Fact]
        public void Test6_is_tool_selected()
        {
            // Arrange
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();
            c1.SelectedTool = ToolType.None;
            Assert.False(c1.IsToolSelected);

            c1.SelectedTool = ToolType.Pencil;
            Assert.True(c1.IsToolSelected);
        }
        [Fact]
        public void Test7_is_drawing_tool()
        {
            // Arrange
            var c1 = new VecEditor.App.ViewModels.MainWindowViewModel();

            c1.SelectedTool = ToolType.Eraser;
            //Assert.False(c1.IsDrawingTool);
            c1.SelectedTool = ToolType.None;
            Assert.False(c1.IsDrawingTool);

            c1.SelectedTool = ToolType.Pencil;
            Assert.True(c1.IsDrawingTool);
            c1.SelectedTool = ToolType.Brush;
            Assert.True(c1.IsDrawingTool);
            c1.SelectedTool = ToolType.Pen;
            //Assert.False(c1.IsDrawingTool);

        }

    }
}