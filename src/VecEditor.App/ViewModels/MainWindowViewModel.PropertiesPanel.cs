using ReactiveUI;
using System;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel
{
    public IReadOnlyList<string> AvailableStrokeColors { get; } =
    [
        "Red",
        "Blue",
        "Green",
        "Black",
        "Orange",
        "Purple",
        "Gray",
        "LightBlue",
        "Yellow"
    ];

    public IReadOnlyList<string> AvailableStrokeThicknesses { get; } =
    [
        "1 px",
        "2 px",
        "3 px",
        "4 px",
        "5 px",
        "8 px",
        "10 px"
    ];

    public string SelectedStrokeColor
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public double SelectedStrokeThickness
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            this.RaisePropertyChanged(nameof(SelectedStrokeThicknessDisplay));
        }
    }

    public string SelectedStrokeThicknessDisplay
    {
        get => $"{SelectedStrokeThickness:0.##} px";
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var cleaned = value.Replace("px", "", StringComparison.OrdinalIgnoreCase).Trim();

            if (double.TryParse(cleaned, out var thickness) && thickness > 0)
            {
                SelectedStrokeThickness = thickness;
                this.RaisePropertyChanged(nameof(SelectedStrokeThicknessDisplay));
            }
        }
    }

    public string CurrentFigureTypeText
    {
        get
        {
            if (SelectedObject is not null)
                return SelectedObject.PrimitiveType;

            if (SelectedPrimitive != PrimitiveType.None)
                return SelectedPrimitive.ToString();

            if (SelectedTool != ToolType.None)
                return SelectedTool.ToString();

            return "Не выбрано";
        }
    }

    public string CurrentXText => GetCurrentBoundsText().x;
    public string CurrentYText => GetCurrentBoundsText().y;
    public string CurrentWidthText => GetCurrentBoundsText().width;
    public string CurrentHeightText => GetCurrentBoundsText().height;

    private (string x, string y, string width, string height) GetCurrentBoundsText()
    {
        if (SelectedObject is not null && SelectedObject.ObjectPoints.Count >= 2)
        {
            var p1 = SelectedObject.ObjectPoints[0];
            var p2 = SelectedObject.ObjectPoints[1];

            var x = Math.Min(p1.X, p2.X);
            var y = Math.Min(p1.Y, p2.Y);
            var width = Math.Abs(p2.X - p1.X);
            var height = Math.Abs(p2.Y - p1.Y);

            return (
                x.ToString("0.##"),
                y.ToString("0.##"),
                width.ToString("0.##"),
                height.ToString("0.##")
            );
        }

        if (IsDrawing && PreviewStartPoint.HasValue && PreviewEndPoint.HasValue)
        {
            var x = Math.Min(PreviewStartPoint.Value.X, PreviewEndPoint.Value.X);
            var y = Math.Min(PreviewStartPoint.Value.Y, PreviewEndPoint.Value.Y);
            var width = Math.Abs(PreviewEndPoint.Value.X - PreviewStartPoint.Value.X);
            var height = Math.Abs(PreviewEndPoint.Value.Y - PreviewStartPoint.Value.Y);

            return (
                x.ToString("0.##"),
                y.ToString("0.##"),
                width.ToString("0.##"),
                height.ToString("0.##")
            );
        }

        if (PrimitiveObjects.Count > 0)
        {
            var last = PrimitiveObjects[^1];

            if (last.ObjectPoints.Count >= 2)
            {
                var p1 = last.ObjectPoints[0];
                var p2 = last.ObjectPoints[1];

                var x = Math.Min(p1.X, p2.X);
                var y = Math.Min(p1.Y, p2.Y);
                var width = Math.Abs(p2.X - p1.X);
                var height = Math.Abs(p2.Y - p1.Y);

                return (
                    x.ToString("0.##"),
                    y.ToString("0.##"),
                    width.ToString("0.##"),
                    height.ToString("0.##")
                );
            }
        }

        return ("—", "—", "—", "—");
    }

    private void RaiseGeometryPropertiesChanged()
    {
        this.RaisePropertyChanged(nameof(CurrentXText));
        this.RaisePropertyChanged(nameof(CurrentYText));
        this.RaisePropertyChanged(nameof(CurrentWidthText));
        this.RaisePropertyChanged(nameof(CurrentHeightText));
    }
}