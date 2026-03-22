using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Windows.Input;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel
{
    public ICommand SelectPointerCommand { get; }
    public ICommand SelectPenCommand { get; }
    public ICommand SelectPencilCommand { get; }
    public ICommand SelectBrushCommand { get; }
    public ICommand SelectEraserCommand { get; }

    public ICommand SelectRectangleCommand { get; }
    public ICommand SelectTriangleCommand { get; }
    public ICommand SelectCircleCommand { get; }
    public ICommand SelectEllipseCommand { get; }
    public ICommand SelectArrowCommand { get; }
    public ICommand SelectLineCommand { get; }

    public ICommand UndoCommand { get; }
    public ICommand RedoCommand { get; }

    private void SelectTool(ToolType tool)
    {
        if (SelectedTool == tool)
        {
            SelectedTool = ToolType.None;

            foreach (var obj in PrimitiveObjects)
                obj.IsSelected = false;

            SelectedObject = null;
            this.RaisePropertyChanged(nameof(PrimitiveObjects));
        }
        else
        {
            SelectedTool = tool;
            SelectedPrimitive = PrimitiveType.None;

            if (tool == ToolType.Pointer)
            {
                foreach (var obj in PrimitiveObjects)
                    obj.IsSelected = false;

                if (PrimitiveObjects.Count > 0)
                {
                    var last = PrimitiveObjects[^1];
                    last.IsSelected = true;
                    SelectedObject = last;
                }
                else
                {
                    SelectedObject = null;
                }

                this.RaisePropertyChanged(nameof(PrimitiveObjects));
            }
        }
    }

    private void SelectPrimitive(PrimitiveType primitive)
    {
        if (SelectedPrimitive == primitive)
        {
            SelectedPrimitive = PrimitiveType.None;
        }
        else
        {
            SelectedPrimitive = primitive;
            SelectedTool = ToolType.None;

            foreach (var obj in PrimitiveObjects)
                obj.IsSelected = false;

            SelectedObject = null;
            this.RaisePropertyChanged(nameof(PrimitiveObjects));
        }
    }
}