using Avalonia;
using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;

namespace VecEditor.ViewModel;

public sealed class MainViewModel : ReactiveObject
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
        public Point ObjectPoint { get; set; }
        public PrimitiveObject(PrimitiveType ptype, Point pt)
        {
            primitiveType = ptype;
            ObjectPoint = pt;
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

    public void add_object(Point tmp)
    {
        if (SelectedPrimitive == PrimitiveType.None)
        {
            return;
        }
        PrimitiveObject tmp_obj = new PrimitiveObject(SelectedPrimitive, tmp); // Заменить на количество точек для текущего примитива
        primitiveObjects.Add(tmp_obj);
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

    // Команды для кнопок примитивов и инструментов
    public ReactiveCommand<PrimitiveType, Unit> SelectPrimitiveCommand { get; }
    public ReactiveCommand<ToolType, Unit> SelectToolCommand { get; }

    // Логические свойства для удобства работы
    public bool IsToolSelected => SelectedTool != ToolType.None;
    public bool IsDrawingTool =>  SelectedTool == ToolType.Pencil ||
                                  SelectedTool == ToolType.Brush;
    

    public MainViewModel()
    {
        //SelectedTool = ToolType.None; // Ничего не выбрано по умолчанию
        SelectedPrimitive = PrimitiveType.None;
        primitiveObjects = new ObservableCollection<PrimitiveObject>();

        // Инициализация команд

        SelectPrimitiveCommand = ReactiveCommand.Create<PrimitiveType>(arg =>
        {
            SelectPrimitive(arg);
        });
        SelectToolCommand = ReactiveCommand.Create<ToolType>(arg =>
        {
            SelectTool(arg);
        });

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

    private void SelectTool(ToolType primitive)
    {
        if (SelectedTool == primitive)
        {
            SelectedTool = ToolType.None;
        }
        else
        {
            SelectedTool = primitive;
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

    public void HandlePrimitiveChanged(PrimitiveType newPrimitive)
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