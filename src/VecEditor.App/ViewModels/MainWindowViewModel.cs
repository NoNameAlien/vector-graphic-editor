using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.Arm;
using VecEditor.ViewModel;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{
    private double _zoom = 1.0;

    public double Zoom
    {
        get => _zoom;
        set
        {
            this.RaiseAndSetIfChanged(ref _zoom, Math.Max(0.1, Math.Min(5.0, value)));
            this.RaisePropertyChanged(nameof(ZoomPercent));
        }
    }

    public string ZoomPercent => $"{Zoom * 100:F0}%";

    
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
        Triangle,
        Circle,
        Ellipse,
        Arrow,
        Line
    }

    private HistoryManager historyManager = new HistoryManager();
    public EditorObjectsState ObjectsState { get; set; } = new();

    public ObservableCollection<PrimitiveObjectState> PrimitiveObjects
        => ObjectsState.PrimitiveObjects;

    public ToolType SelectedTool
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);

            this.RaisePropertyChanged(nameof(IsPointerActive));
            this.RaisePropertyChanged(nameof(IsPenActive));
            this.RaisePropertyChanged(nameof(IsPencilActive));
            this.RaisePropertyChanged(nameof(IsBrushActive));
            this.RaisePropertyChanged(nameof(IsEraserActive));
            this.RaisePropertyChanged(nameof(IsToolSelected));
            this.RaisePropertyChanged(nameof(IsDrawingTool));

            this.RaisePropertyChanged(nameof(CurrentFigureTypeText));
        }
    }

    public PrimitiveType SelectedPrimitive
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);

            this.RaisePropertyChanged(nameof(IsRectangleActive));
            this.RaisePropertyChanged(nameof(IsTriangleActive));
            this.RaisePropertyChanged(nameof(IsCircleActive));
            this.RaisePropertyChanged(nameof(IsEllipseActive));
            this.RaisePropertyChanged(nameof(IsArrowActive));
            this.RaisePropertyChanged(nameof(IsLineActive));

            this.RaisePropertyChanged(nameof(CurrentFigureTypeText));
        }
    }

    public PrimitiveObjectState? SelectedObject
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

    public MainWindowViewModel()
    {
        SelectedTool = ToolType.None;
        SelectedPrimitive = PrimitiveType.Line;
        SelectedStrokeColor = "Red";
        SelectedStrokeThickness = 2;

        SelectPointerCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectTool(ToolType.Pointer));
        SelectPenCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectTool(ToolType.Pen));
        SelectPencilCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectTool(ToolType.Pencil));
        SelectBrushCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectTool(ToolType.Brush));
        SelectEraserCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectTool(ToolType.Eraser));

        SelectRectangleCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectPrimitive(PrimitiveType.Rectangle));
        SelectTriangleCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectPrimitive(PrimitiveType.Triangle));
        SelectCircleCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectPrimitive(PrimitiveType.Circle));
        SelectEllipseCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectPrimitive(PrimitiveType.Ellipse));
        SelectArrowCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectPrimitive(PrimitiveType.Arrow));
        SelectLineCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => SelectPrimitive(PrimitiveType.Line));

        UndoCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => Undo());
        RedoCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() => Redo());

        NewProjectCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await NewProjectAsync());
        OpenCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await OpenAsync());
        SaveCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await SaveAsync());
        SaveAsCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(async () => await SaveAsAsync());
        ZoomInCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(ZoomIn);
        ZoomOutCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(ZoomOut);
        ZoomResetCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(ZoomReset);
    }

    private void Exit()
    {
        // Закрыть приложение
        Environment.Exit(0);
    }
}