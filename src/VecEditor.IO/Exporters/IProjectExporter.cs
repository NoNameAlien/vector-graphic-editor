using System.Collections.Generic;
using System.IO;
using VecEditor.Core.Figures;

namespace VecEditor.IO.Exporters;

public interface IProjectExporter
{
    void Export(IEnumerable<IFigure> figures, Stream stream);
}