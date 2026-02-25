
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel:ReactiveObject
{
    [Reactive] public partial string Greeting { get; set; } = "VecEditor";

    public ReactiveCommand<Unit,Unit> Command { get; set; }

    int count = 0;
    public MainWindowViewModel()
    {
        Command = ReactiveCommand.Create<Unit,Unit>(_ => { Greeting = count++.ToString(); return default; }, 
            this.WhenAnyValue(t=>t.Greeting).Select(g=>g != "2"));
    }

    public VecEditor.ViewModel.MainViewModel Editor { get; } = new();
}
