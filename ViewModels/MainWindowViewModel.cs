namespace vector_graphic_editor.ViewModels;

public class MainWindowViewModel
{
    public string Greeting => "VecEditor";

    public VecEditor.ViewModel.MainViewModel Editor { get; } = new();
}