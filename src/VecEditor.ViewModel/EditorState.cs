namespace VecEditor.ViewModel;

public sealed class EditorState : NotifyBase
{
    private string _selectedTool = "Pointer";
    public string SelectedTool
    {
        get => _selectedTool;
        set => Set(ref _selectedTool, value);
    }

    private string _selectedPrimitive = "Line";
    public string SelectedPrimitive
    {
        get => _selectedPrimitive;
        set => Set(ref _selectedPrimitive, value);
    }
}