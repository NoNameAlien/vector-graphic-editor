using System.Collections.Generic;
using VecEditor.Core.Figures;

namespace VecEditor.IO;

public sealed class ProjectDocument
{
    public string Version { get; set; } = "1.0";
    public DateTime LastModified { get; set; } = DateTime.Now;  
    public List<IFigure> Figures { get; set; } = new();
}