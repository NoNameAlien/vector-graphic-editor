using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace VecEditor.App.Converters;

public sealed class StringToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string colorName && !string.IsNullOrWhiteSpace(colorName))
        {
            try
            {
                return Brush.Parse(colorName);
            }
            catch
            {
                return Brushes.Transparent;
            }
        }

        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ISolidColorBrush brush)
            return brush.Color.ToString();

        return "Transparent";
    }
}