using Avalonia;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using VecEditor.ViewModel;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel
{
    private readonly List<Point> _tempPoints = new();

    public Point? PreviewStartPoint
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            RaiseGeometryPropertiesChanged();
        }
    }

    public Point? PreviewEndPoint
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            RaiseGeometryPropertiesChanged();
        }
    }

    public bool IsDrawing
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            RaiseGeometryPropertiesChanged();
        }
    }

    public void AddPoint(Point point)
    {
        if (SelectedPrimitive == PrimitiveType.None)
            return;

        if (!IsDrawing)
        {
            _tempPoints.Clear();
            _tempPoints.Add(point);

            PreviewStartPoint = point;
            PreviewEndPoint = point;
            IsDrawing = true;
            return;
        }

        _tempPoints.Add(point);

        var primitive = new PrimitiveObjectState
        (
            SelectedPrimitive,
            SelectedTool,
            _tempPoints,
            SelectedStrokeColor,
            SelectedStrokeThickness
        );

        PrimitiveObjects.Add(primitive);
        historyManager.addState(ObjectsState);


        _tempPoints.Clear();
        PreviewStartPoint = null;
        PreviewEndPoint = null;
        IsDrawing = false;


        RaiseGeometryPropertiesChanged();
    }

    public void UpdatePreview(Point point)
    {
        if (!IsDrawing || _tempPoints.Count == 0)
            return;

        PreviewStartPoint = _tempPoints[0];
        PreviewEndPoint = point;
    }
}