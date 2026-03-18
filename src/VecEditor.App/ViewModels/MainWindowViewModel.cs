using Avalonia;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{

    public enum ToolType
    {
        None,
        Pointer,
        Pen,
        Pencil,
        Brush,
        Eraser
    }

    public enum PrimitiveType
    {
        None,
        Rectangle,
        Circle,
        Ellipse,
        Triangle,
        Arrow,
        Line
    }

    public ToolType SelectedTool
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            this.RaisePropertyChanged(nameof(CurrentFigureTypeText));
            this.RaisePropertyChanged(nameof(HasFigureSettings));
        }
    }

    public PrimitiveType SelectedPrimitive
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            this.RaisePropertyChanged(nameof(CurrentFigureTypeText));
            this.RaisePropertyChanged(nameof(HasFigureSettings));
        }
    }

    // Список примитивов
    public VecEditor.ViewModel.PrimitiveObjectState? SelectedObject
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            this.RaisePropertyChanged(nameof(HasSelectedObject));
            this.RaisePropertyChanged(nameof(CurrentFigureTypeText));
            this.RaisePropertyChanged(nameof(CurrentXText));
            this.RaisePropertyChanged(nameof(CurrentYText));
            this.RaisePropertyChanged(nameof(CurrentWidthText));
            this.RaisePropertyChanged(nameof(CurrentHeightText));
        }
    }

    public bool HasSelectedObject => SelectedObject is not null;
    public VecEditor.ViewModel.EditorObjectsState ObjectsState { get; } = new();
    public ObservableCollection<VecEditor.ViewModel.PrimitiveObjectState> PrimitiveObjects
        => ObjectsState.PrimitiveObjects;

    private readonly List<Point> _tempPoints = new();

    public void AddPoint(Point point)
    {
        if (SelectedPrimitive != PrimitiveType.Line && SelectedPrimitive != PrimitiveType.Rectangle)
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

        var primitive = new VecEditor.ViewModel.PrimitiveObjectState
        {
            PrimitiveType = SelectedPrimitive.ToString(),
            ToolType = SelectedTool.ToString(),
            ObjectPoints = _tempPoints
        .Take(2)
        .Select(p => new VecEditor.Core.Geometry.Point2(p.X, p.Y))
        .ToList(),
            StrokeColor = SelectedStrokeColor,
            StrokeThickness = SelectedStrokeThickness
        };

        PrimitiveObjects.Add(primitive);

        RaiseGeometryPropertiesChanged();
        _tempPoints.Clear();
        PreviewStartPoint = null;
        PreviewEndPoint = null;
        IsDrawing = false;
    }

    public void UpdatePreview(Point point)
    {
        if (!IsDrawing || _tempPoints.Count == 0)
            return;

        PreviewStartPoint = _tempPoints[0];
        PreviewEndPoint = point;
    }

    public void SelectObjectAt(Point point)
    {
        foreach (var obj in PrimitiveObjects)
            obj.IsSelected = false;

        SelectedObject = null;

        foreach (var obj in PrimitiveObjects.Reverse())
        {
            if (obj.PrimitiveType == "Line" && obj.ObjectPoints.Count >= 2)
            {
                var p1 = obj.ObjectPoints[0];
                var p2 = obj.ObjectPoints[1];

                if (IsPointNearLine(point, p1, p2, 6))
                {
                    obj.IsSelected = true;
                    SelectedObject = obj;
                    break;
                }
            }
        }

        this.RaisePropertyChanged(nameof(PrimitiveObjects));
    }

    // Свойства для UI
    public bool IsPointerActive => SelectedTool == ToolType.Pointer;
    public bool IsPenActive => SelectedTool == ToolType.Pen;
    public bool IsPencilActive => SelectedTool == ToolType.Pencil;
    public bool IsBrushActive => SelectedTool == ToolType.Brush;
    public bool IsEraserActive => SelectedTool == ToolType.Eraser;

    public bool IsRectangleActive => SelectedPrimitive == PrimitiveType.Rectangle;
    public bool IsTriangleActive => SelectedPrimitive == PrimitiveType.Triangle;
    public bool IsCircleActive => SelectedPrimitive == PrimitiveType.Circle;
    public bool IsEllipseActive => SelectedPrimitive == PrimitiveType.Ellipse;
    public bool IsArrowActive => SelectedPrimitive == PrimitiveType.Arrow;
    public bool IsLineActive => SelectedPrimitive == PrimitiveType.Line;

    public bool IsDrawing
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            RaiseGeometryPropertiesChanged();
        }
    }

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

    public string CurrentXText => GetCurrentBoundsText().x;
    public string CurrentYText => GetCurrentBoundsText().y;
    public string CurrentWidthText => GetCurrentBoundsText().width;
    public string CurrentHeightText => GetCurrentBoundsText().height;

    public bool HasFigureSettings => SelectedPrimitive != PrimitiveType.None || SelectedTool != ToolType.None;

    // Команды для каждой кнопки
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

    // Логические свойства для удобства работы
    public bool IsToolSelected => SelectedTool != ToolType.None;
    public bool IsDrawingTool => SelectedTool == ToolType.Pen ||
                                  SelectedTool == ToolType.Pencil ||
                                  SelectedTool == ToolType.Brush;

    public MainWindowViewModel()
    {
        SelectedTool = ToolType.None;
        SelectedPrimitive = PrimitiveType.Line;
        SelectedStrokeColor = "Red";
        SelectedStrokeThickness = 2;

        // Инициализация команд
        SelectPointerCommand = new RelayCommand(() => SelectTool(ToolType.Pointer));
        SelectPenCommand = new RelayCommand(() => SelectTool(ToolType.Pen));
        SelectPencilCommand = new RelayCommand(() => SelectTool(ToolType.Pencil));
        SelectBrushCommand = new RelayCommand(() => SelectTool(ToolType.Brush));
        SelectEraserCommand = new RelayCommand(() => SelectTool(ToolType.Eraser));
        
        SelectRectangleCommand = new RelayCommand(() => SelectPrimitive(PrimitiveType.Rectangle));
        SelectTriangleCommand = new RelayCommand(() => SelectPrimitive(PrimitiveType.Triangle));
        SelectCircleCommand = new RelayCommand(() => SelectPrimitive(PrimitiveType.Circle));
        SelectEllipseCommand = new RelayCommand(() => SelectPrimitive(PrimitiveType.Ellipse));
        SelectArrowCommand = new RelayCommand(() => SelectPrimitive(PrimitiveType.Arrow));
        SelectLineCommand = new RelayCommand(() => SelectPrimitive(PrimitiveType.Line));

        // Реакция на изменение инструмента
        this.WhenAnyValue(x => x.SelectedTool)
            .Subscribe(tool =>
            {
                // Обновляем UI свойства
                this.RaisePropertyChanged(nameof(IsPointerActive));
                this.RaisePropertyChanged(nameof(IsPenActive));
                this.RaisePropertyChanged(nameof(IsPencilActive));
                this.RaisePropertyChanged(nameof(IsBrushActive));
                this.RaisePropertyChanged(nameof(IsEraserActive));
                this.RaisePropertyChanged(nameof(IsToolSelected));
                this.RaisePropertyChanged(nameof(IsDrawingTool));

                // Здесь можно добавить дополнительную логику
                HandleToolChanged(tool);
            });

        this.WhenAnyValue(x => x.SelectedPrimitive)
            .Subscribe(primitive =>
            {
                // Обновляем UI свойства
                this.RaisePropertyChanged(nameof(IsRectangleActive));
                this.RaisePropertyChanged(nameof(IsTriangleActive));
                this.RaisePropertyChanged(nameof(IsCircleActive));
                this.RaisePropertyChanged(nameof(IsEllipseActive));
                this.RaisePropertyChanged(nameof(IsLineActive));
                this.RaisePropertyChanged(nameof(IsArrowActive));

                // Здесь можно добавить дополнительную логику
                HandlePrimitiveChanged(primitive);
            });
    }

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
        }
    }

    private void HandleToolChanged(ToolType newTool)
    {
        // Здесь логика 
        switch (newTool)
        {
            case ToolType.Pencil:
                // Активировать карандаш
                break;
            case ToolType.Brush:
                // Активировать кисть
                break;
            case ToolType.Eraser:
                // Активировать ластик
                break;
            case ToolType.None:
                // Отключить все инструменты
                break;
        }
    }

    private void HandlePrimitiveChanged(PrimitiveType newPrimitive)
    {
        // Здесь логика 
        switch (newPrimitive)
        {
            case PrimitiveType.Rectangle:
                // 
                break;
            case PrimitiveType.Circle:
                // 
                break;
            case PrimitiveType.Ellipse:
                // 
                break;
            case PrimitiveType.Triangle:
                // 
                break;
            case PrimitiveType.Arrow:
                //
                break;
            case PrimitiveType.None:
                // Отключить все 
                break;
        }
    }

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

    private static bool IsPointNearLine(Point point, VecEditor.Core.Geometry.Point2 a, VecEditor.Core.Geometry.Point2 b, double tolerance)
    {
        var px = point.X;
        var py = point.Y;
        var x1 = a.X;
        var y1 = a.Y;
        var x2 = b.X;
        var y2 = b.Y;

        var dx = x2 - x1;
        var dy = y2 - y1;

        if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon)
            return Math.Sqrt((px - x1) * (px - x1) + (py - y1) * (py - y1)) <= tolerance;

        var t = ((px - x1) * dx + (py - y1) * dy) / (dx * dx + dy * dy);
        t = Math.Max(0, Math.Min(1, t));

        var nearestX = x1 + t * dx;
        var nearestY = y1 + t * dy;

        var dist = Math.Sqrt((px - nearestX) * (px - nearestX) + (py - nearestY) * (py - nearestY));
        return dist <= tolerance;
    }
}