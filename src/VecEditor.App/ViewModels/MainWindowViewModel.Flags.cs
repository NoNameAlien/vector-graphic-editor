namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel
{
    public bool IsPointerActive => SelectedTool == ToolType.Pointer;
    public bool IsPenActive => SelectedTool == ToolType.Pen;
    public bool IsPencilActive => SelectedTool == ToolType.Pencil;
    public bool IsBrushActive => SelectedTool == ToolType.Brush;
    public bool IsEraserActive => SelectedTool == ToolType.Eraser;

    public bool IsRectangleActive => SelectedPrimitive == PrimitiveType.Rectangle;
    public bool IsTriangleActive => SelectedPrimitive == PrimitiveType.Triangle;
    public bool IsCircleActive => SelectedPrimitive == PrimitiveType.Circle;
    public bool IsEllipseActive => SelectedPrimitive == PrimitiveType.Ellipse;
    public bool IsArrowActive => SelectedPrimitive == PrimitiveType.Arrow;
    public bool IsLineActive => SelectedPrimitive == PrimitiveType.Line;

    public bool IsToolSelected => SelectedTool != ToolType.None;
    public bool IsDrawingTool =>
        SelectedTool == ToolType.Pen ||
        SelectedTool == ToolType.Pencil ||
        SelectedTool == ToolType.Brush ||
        SelectedTool == ToolType.Eraser;

    public bool HasSelectedObject => SelectedObject is not null;
}