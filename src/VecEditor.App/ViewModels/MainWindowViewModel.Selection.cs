using Avalonia;
using ReactiveUI;
using System;
using System.Linq;
using VecEditor.Core.Geometry;
using VecEditor.ViewModel;

namespace VecEditor.App.ViewModels;

public partial class MainWindowViewModel
{
    public void SelectObjectAt(Point point)
    {
        foreach (var obj in PrimitiveObjects)
            obj.IsSelected = false;

        SelectedObject = null;

        foreach (var obj in PrimitiveObjects.Reverse())
        {
            if (obj.PrimitiveType == "Line" && obj.ObjectPoints.Count >= 2)
            {
                var p1 = obj.ObjectPoints[0];
                var p2 = obj.ObjectPoints[1];

                if (IsPointNearLine(point, p1, p2, 6))
                {
                    obj.IsSelected = true;
                    SelectedObject = obj;
                    break;
                }
            }
            if (obj.PrimitiveType == "Rectangle" || obj.PrimitiveType == "Triangle" 
                || obj.PrimitiveType == "Ellipse" || obj.PrimitiveType == "Circle" && obj.ObjectPoints.Count >= 2)
            {
                var p1 = obj.ObjectPoints[0];
                var p2 = obj.ObjectPoints[1];

                if (IsPointinsideSqare(point, p1, p2))
                {
                    obj.IsSelected = true;
                    SelectedObject = obj;
                    break;
                }
            }
        }

        this.RaisePropertyChanged(nameof(PrimitiveObjects));
    }

    public void DeleteObjectAt(Point point)
    {
        SelectObjectAt(point);
        if (SelectedObject != null)
        {
            PrimitiveObjects.Remove(SelectedObject);
            SelectedObject = null;
        }
        this.RaisePropertyChanged(nameof(PrimitiveObjects));
    }

    private static bool IsPointNearLine(Point point, Point2 a, Point2 b, double tolerance)
    {
        var px = point.X;
        var py = point.Y;
        var x1 = a.X;
        var y1 = a.Y;
        var x2 = b.X;
        var y2 = b.Y;

        var dx = x2 - x1;
        var dy = y2 - y1;

        if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon)
            return Math.Sqrt((px - x1) * (px - x1) + (py - y1) * (py - y1)) <= tolerance;

        var t = ((px - x1) * dx + (py - y1) * dy) / (dx * dx + dy * dy);
        t = Math.Max(0, Math.Min(1, t));

        var nearestX = x1 + t * dx;
        var nearestY = y1 + t * dy;

        var dist = Math.Sqrt((px - nearestX) * (px - nearestX) + (py - nearestY) * (py - nearestY));
        return dist <= tolerance;
    }
    private static bool IsPointinsideSqare(Point point, Point2 a, Point2 b)
    {
        var px = point.X;
        var py = point.Y;
        var x1 = a.X;
        var y1 = a.Y;
        var x2 = b.X;
        var y2 = b.Y;
        if(x1 > x2)
        {
            var tmp = x1;
            x1 = x2;
            x2 = tmp;
        }
        if (y1 > y2)
        {
            var tmp = y1;
            y1 = y2;
            y2 = tmp;
        }

        var dx = x2 - x1;
        var dy = y2 - y1;

        if (x1 <= px && px <= x2 && y1 <= py && py <= y2)
        {
            return true;
        }
        return false;
    }
}