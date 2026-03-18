using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using VecEditor.Core.Geometry;

namespace VecEditor.App.Converters;

public sealed class Point2ToAvaloniaPointConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Point2 p)
            return new Point(p.X, p.Y);

        return new Point();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}