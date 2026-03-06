using System.Collections.Generic;
using System.IO;
using SkiaSharp;
using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;

namespace VecEditor.IO.Exporters;

public class PngExporter : IProjectExporter
{
    private const int Margin = 20;

    public void Export(IEnumerable<IFigure> figures, Stream stream)
    {
        var bounds = GetTotalBounds(figures);
        if (bounds.Width <= 0 || bounds.Height <= 0) return;

        int width = (int)(bounds.Width + 2 * Margin);
        int height = (int)(bounds.Height + 2 * Margin);

        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);
        canvas.Translate((float)(Margin - bounds.X), (float)(Margin - bounds.Y));

        foreach (var figure in figures)
        {
            if (figure is LineFigure line)
                DrawLine(canvas, line);
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        data.SaveTo(stream);
    }

    private static void DrawLine(SKCanvas canvas, LineFigure line)
    {
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 1,
            IsStroke = true
        };
        canvas.DrawLine((float)line.A.X, (float)line.A.Y, (float)line.B.X, (float)line.B.Y, paint);
    }

    private static RectD GetTotalBounds(IEnumerable<IFigure> figures)
    {
        double minX = double.MaxValue, minY = double.MaxValue;
        double maxX = double.MinValue, maxY = double.MinValue;
        foreach (var fig in figures)
        {
            var b = fig.Bounds;
            minX = System.Math.Min(minX, b.X);
            minY = System.Math.Min(minY, b.Y);
            maxX = System.Math.Max(maxX, b.X + b.Width);
            maxY = System.Math.Max(maxY, b.Y + b.Height);
        }
        return new RectD(minX, minY, maxX - minX, maxY - minY);
    }
}