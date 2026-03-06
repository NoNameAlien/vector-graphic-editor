namespace VecEditor.IO.Abstractions;

public interface IFileService
{
    Task<string?> OpenFileAsync(string? filter = null);
    Task<string?> SaveFileAsync(string? filter = null, string? defaultFileName = null);
}