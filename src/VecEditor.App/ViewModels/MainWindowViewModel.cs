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
        Pen,
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
    public bool IsPenActive => SelectedTool == ToolType.Pen;
    public bool IsPencilActive => SelectedTool == ToolType.Pencil;
    public bool IsBrushActive => SelectedTool == ToolType.Brush;
    public bool IsEraserActive => SelectedTool == ToolType.Eraser;

    // Команды для каждой кнопки
    public ICommand SelectPenCommand { get; }
    public ICommand SelectPencilCommand { get; }
    public ICommand SelectBrushCommand { get; }
    public ICommand SelectEraserCommand { get; }

    // Логические свойства для удобства работы
    public bool IsToolSelected => SelectedTool != ToolType.None;
    public bool IsDrawingTool => SelectedTool == ToolType.Pen ||
                                  SelectedTool == ToolType.Pencil ||
                                  SelectedTool == ToolType.Brush;

    public MainWindowViewModel()
    {
        SelectedTool = ToolType.None; // Ничего не выбрано по умолчанию

        // Инициализация команд
        SelectPenCommand = new RelayCommand(() => SelectTool(ToolType.Pen));
        SelectPencilCommand = new RelayCommand(() => SelectTool(ToolType.Pencil));
        SelectBrushCommand = new RelayCommand(() => SelectTool(ToolType.Brush));
        SelectEraserCommand = new RelayCommand(() => SelectTool(ToolType.Eraser));

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
            case ToolType.Pen:
                // Активировать перо
                break;
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
}