using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Data.Converters;
using Avalonia.Input;
using System.Globalization;
using VecEditor.ViewModel;

namespace VecEditor.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Resources.Add("TypeToBool", new EnumToBoolConverter());
    }
    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is MainViewModel viewModel)
        {
            // Получаем координаты относительно Canvas
            var point = e.GetPosition((Visual?)sender);
            viewModel.add_object(point);
            double x = point.X;
            double y = point.Y;

            System.Diagnostics.Debug.WriteLine($"Клик: X={x}, Y={y}");
        }
    }
}

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Сравниваем значение Enum с параметром из XAML (ConverterParameter)
        if (value == null || parameter == null) return false;

        return value.ToString() == parameter.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}