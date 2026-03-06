using Avalonia;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{

    public enum ToolType
    {
        None,
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

    // Контейнер для примитива
    public class PrimitiveObject
    {
        public PrimitiveType primitiveType { get; set; }
        public ToolType toolType { get; set; }
        public List<Point> ObjectPoints { get; set; }
        public PrimitiveObject(PrimitiveType pt, ToolType tt, List<Point> pts)
        {
            primitiveType = pt;
            toolType = tt;
            ObjectPoints = pts;
        }
    }

    public ToolType SelectedTool
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public PrimitiveType SelectedPrimitive
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    // Список примитивов
    public ObservableCollection<PrimitiveObject> primitiveObjects
    {
        get => field;
        set => field = value;
    }

    public List<Point> temp_points
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value.ToList());
    }

    public void add_point(Point tmp)
    {
        if (SelectedPrimitive != PrimitiveType.Line)
        {
            return;
        }
        temp_points.Add(tmp);
        if (temp_points.Count >= 2) // Заменить на количество точек для текущего примитива
        {
            PrimitiveObject tmp_obj = new PrimitiveObject(SelectedPrimitive, SelectedTool, temp_points.GetRange(0, 2)); // Заменить на количество точек для текущего примитива
            primitiveObjects.Add(tmp_obj);
            temp_points.RemoveRange(0, 2);
        }
    }
    
    // Свойства для UI
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

    public System.Drawing.Point Start, End;

    // Команды для каждой кнопки
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
        SelectedTool = ToolType.None; // Ничего не выбрано по умолчанию
        SelectedPrimitive = PrimitiveType.None;
        primitiveObjects = new ObservableCollection<PrimitiveObject>();
        temp_points = new List<Point>();

        // Инициализация команд
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
        }
        else
        {
            SelectedTool = tool;
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
}