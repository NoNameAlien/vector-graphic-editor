using System.Linq;
using Avalonia.Controls;
using VecEditor.IO.Abstractions;

namespace VecEditor.IO.Services;

public class FileService : IFileService
{
    private readonly Window _target;

    public FileService(Window target) => _target = target;

    public async Task<string?> OpenFileAsync(string? filter = null)
    {
        var dialog = new OpenFileDialog();
        if (!string.IsNullOrEmpty(filter))
            dialog.Filters = ParseFilter(filter);
        var result = await dialog.ShowAsync(_target);
        return result?.FirstOrDefault();
    }

    public async Task<string?> SaveFileAsync(string? filter = null, string? defaultFileName = null)
    {
        var dialog = new SaveFileDialog { InitialFileName = defaultFileName };
        if (!string.IsNullOrEmpty(filter))
            dialog.Filters = ParseFilter(filter);
        return await dialog.ShowAsync(_target);
    }

    private static List<FileDialogFilter> ParseFilter(string filter)
    {
        var list = new List<FileDialogFilter>();
        var parts = filter.Split('|');
        for (int i = 0; i < parts.Length; i += 2)
        {
            if (i + 1 < parts.Length)
            {
                list.Add(new FileDialogFilter
                {
                    Name = parts[i],
                    Extensions = parts[i + 1].Split(';').Select(e => e.TrimStart('*', '.')).ToList()
                });
            }
        }
        return list;
    }
}