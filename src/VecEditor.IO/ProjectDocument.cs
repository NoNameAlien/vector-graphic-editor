using System.Collections.Generic;
using VecEditor.Core.Figures;

namespace VecEditor.IO;

public sealed class ProjectDocument
{
    public List<IFigure> Figures { get; set; } = new();
}