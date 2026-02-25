using Avalonia.Controls;
using Avalonia.Controls.Templates;
using src.VecEditor.App.ViewModels;
using System;
using VecEditor.App.ViewModels;

namespace VecEditor.App.ViewModels;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        if (data is null) return new TextBlock { Text = "null" };

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
            return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data) => data is ViewModelBase;
}