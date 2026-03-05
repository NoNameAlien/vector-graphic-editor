using Avalonia;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Reactive;
using System.Windows.Input;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{
    public enum ToolType
    {
        None,
        Pencil,
        Brush,
        Eraser
    }

    private ToolType _selectedTool;

    public ToolType SelectedTool
    {
        get => _selectedTool;
        set => this.RaiseAndSetIfChanged(ref _selectedTool, value);
    }

    // Свойства для UI
    public bool IsPencilActive => SelectedTool == ToolType.Pencil;
    public bool IsBrushActive => SelectedTool == ToolType.Brush;
    public bool IsEraserActive => SelectedTool == ToolType.Eraser;

    // Команды для каждой кнопки
    public ICommand SelectPencilCommand { get; }
    public ICommand SelectBrushCommand { get; }
    public ICommand SelectEraserCommand { get; }

    public bool IsToolSelected => SelectedTool != ToolType.None;
    public bool IsDrawingTool => SelectedTool == ToolType.Pencil || SelectedTool == ToolType.Brush;

    public MainWindowViewModel()
    {
        SelectedTool = ToolType.None; // Ничего не выбрано по умолчанию

        tmp_points = new List<Point>();
        objects = new List<prim_obj>();

        // Инициализация команд
        SelectPencilCommand = new RelayCommand(() => SelectTool(ToolType.Pencil));
        SelectBrushCommand = new RelayCommand(() => SelectTool(ToolType.Brush));  // Не надо, это из тулкид ReactiveComamand.Create
        // ObservableAsPropertyHelper можно посмотреть
        SelectEraserCommand = new RelayCommand(() => SelectTool(ToolType.Eraser));

        // Реакция на изменение инструмента
        this.WhenAnyValue(x => x.SelectedTool)
            .Subscribe(tool =>
            {
                // Обновляем UI свойства
                this.RaisePropertyChanged(nameof(IsPencilActive));
                this.RaisePropertyChanged(nameof(IsBrushActive));
                this.RaisePropertyChanged(nameof(IsEraserActive));
                this.RaisePropertyChanged(nameof(IsToolSelected));
                this.RaisePropertyChanged(nameof(IsDrawingTool));

                HandleToolChanged(tool);
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

    public void Add_point(Point point)
    {
        tmp_points.Add(point);
        if (tmp_points.Count >= 2)
        {
            prim_obj prim_ = new prim_obj();
            prim_.A = tmp_points[0];
            prim_.B = tmp_points[1];
            prim_.type = "Line";
            objects.Add(prim_);
            tmp_points.RemoveRange(0, 2);
        }
    }

    public struct prim_obj
    {
        public string type;
        public Point A;
        public Point B;
    }

    public List<Point> tmp_points { get; set; }
    public List<prim_obj> objects { get; set; }

    int count = 0;
  
    public int Cnt()
    {
        return 0;
    }

}