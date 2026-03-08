using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using VecEditor.ViewModel;

namespace VecEditor.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is MainViewModel viewModel)
        {
            // Получаем координаты относительно Canvas
            var point = e.GetPosition((Visual?)sender);
            viewModel.add_point(point);
            double x = point.X;
            double y = point.Y;

            System.Diagnostics.Debug.WriteLine($"Клик: X={x}, Y={y}");
        }
    }
}