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
            viewModel.AddPoint(point);
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
}