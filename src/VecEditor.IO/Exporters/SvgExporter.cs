using System.Collections.Generic;
using System.IO;
using Svg;
using VecEditor.Core.Figures;

namespace VecEditor.IO.Exporters;

public class SvgExporter : IProjectExporter
{
    public void Export(IEnumerable<IFigure> figures, Stream stream)
    {
        var svgDoc = new SvgDocument();
        foreach (var figure in figures)
        {
            if (figure is LineFigure line)
            {
                svgDoc.Children.Add(new SvgLine
                {
                    StartX = (float)line.A.X,
                    StartY = (float)line.A.Y,
                    EndX = (float)line.B.X,
                    EndY = (float)line.B.Y,
                    Stroke = new SvgColourServer(System.Drawing.Color.Black)
                });
            }
        }
        svgDoc.Write(stream);
    }
}