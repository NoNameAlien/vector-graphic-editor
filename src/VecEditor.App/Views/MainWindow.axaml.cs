using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using VecEditor.App.ViewModels;

namespace VecEditor.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            var point = e.GetPosition((Visual?)sender);

            if (viewModel.SelectedTool == MainWindowViewModel.ToolType.Pointer)
            {
                viewModel.SelectObjectAt(point);
            }
            else if (viewModel.SelectedTool == MainWindowViewModel.ToolType.Eraser)
            {
                viewModel.DeleteObjectAt(point);
            }
            else
            {
                viewModel.AddPoint(point);
            }
        }
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            var point = e.GetPosition((Visual?)sender);
            viewModel.UpdatePreview(point);
        }
    }

    private void StrokeThicknessTextBox_GotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.CaretIndex = textBox.Text?.Length ?? 0;
            textBox.SelectionStart = textBox.CaretIndex;
            textBox.SelectionEnd = textBox.CaretIndex;
        }
    }
}