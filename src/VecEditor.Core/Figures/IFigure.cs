using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using VecEditor.Core.Geometry;

namespace VecEditor.Core.Figures;

public static class FigurePrototipes
{
    //public static Dictionary<string, IFigure> AvailableFigures = new()
    //{
    //    ["Circle"] = new Circle()
    //};
    private class FigureMetadata
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public string Name { get; set; } = "";
        // ReSharper disable once ReplaceAutoPropertyWithComputedProperty
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    }
    private class ImportInfo
    {
        [ImportMany]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public IEnumerable<ExportFactory<IFigure, FigureMetadata>> AvailableFiguresImport { get; set; }
            = Enumerable.Empty<ExportFactory<IFigure, FigureMetadata>>();
    }

    private static readonly ImportInfo info;

    public static IEnumerable<string> AvailableFigures =>
        info.AvailableFiguresImport.Select(p => p.Metadata.Name);
    public static IFigure CreateFigure(string name)
    {
        var creator = info.AvailableFiguresImport.First(s => s.Metadata.Name == name);
        return creator.CreateExport().Value;
    }
    static FigurePrototipes()
    {
        IEnumerable<Assembly> assemblies = [typeof(LineFigure).Assembly];
        var conf = new ContainerConfiguration();
        try
        {
            conf = conf.WithAssemblies(assemblies);
        }
        catch (Exception)
        {
            // ignored
        }

        var cont = conf.CreateContainer();
        info = new();
        cont.SatisfyImports(info);
    }
}

public interface IFigure
{
    Guid Id { get; }
    RectD Bounds { get; }

    IFigure Clone();
}
