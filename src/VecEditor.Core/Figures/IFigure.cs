using System;
using VecEditor.Core.Geometry;

namespace VecEditor.Core.Figures;

public interface IFigure
{
    Guid Id { get; }
    RectD Bounds { get; }
}