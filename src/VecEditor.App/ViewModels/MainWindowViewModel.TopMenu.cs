using DynamicData;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VecEditor.Core.Figures;
using VecEditor.Core.Geometry;
using VecEditor.IO;
using VecEditor.IO.Exporters;
using VecEditor.IO.Services;
using VecEditor.ViewModel;

namespace VecEditor.App.ViewModels
{
    public partial class MainWindowViewModel
    {
        private FileService? _fileService;
        private JsonProjectSerializer _serializer = new JsonProjectSerializer();
        private string? _currentFilePath;

        public void SetFileService(FileService fileService)
        {
            _fileService = fileService;
        }
        public void Undo()
        {
            if (historyManager.CheckNotEmpty())
            {
                PrimitiveObjects.Clear();

                foreach (var obj in (historyManager.getPrewState()).PrimitiveObjects)
                {
                    PrimitiveObjects.Add(obj);
                }
                this.RaisePropertyChanged(nameof(PrimitiveObjects));
            }
        }
        public void Redo() 
        {
            if (historyManager.CheckNotEmpty())
            {
                PrimitiveObjects.Clear();

                foreach (var obj in (historyManager.getNextState()).PrimitiveObjects)
                {
                    PrimitiveObjects.Add(obj);
                }
                this.RaisePropertyChanged(nameof(PrimitiveObjects));
            }
        }
        public async Task OpenAsync(string? filePath = null)
        {
            System.Diagnostics.Debug.WriteLine("=== OpenAsync ВЫЗВАН ==="); // ДОБАВЬ ЭТО
            try
            {
                if (_fileService == null)
                {
                    await ShowMessage("Ошибка", "FileService не инициализирован");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("_fileService не null, вызываем OpenFileAsync"); // ДОБАВЬ ЭТО
                // Если путь не указан, показываем диалог выбора файла
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = await _fileService.OpenFileAsync("VecEditor Project|*.json");
                    if (string.IsNullOrEmpty(filePath))
                        return;
                }

                var json = await File.ReadAllTextAsync(filePath);
                var project = _serializer.Deserialize(json);

                // Загружаем фигуры из проекта
                PrimitiveObjects.Clear();
                foreach (var figure in project.Figures)
                {
                    // Конвертируем IFigure в PrimitiveObjectState
                    var primitive = ConvertFigureToPrimitive(figure);
                    if (primitive != null)
                    {
                        PrimitiveObjects.Add(primitive);
                    }
                }

                _currentFilePath = filePath;
                this.RaisePropertyChanged(nameof(PrimitiveObjects));

                // Сохраняем состояние в историю
                historyManager.addState(ObjectsState);

                await ShowMessage("Успех", $"Проект загружен из: {filePath}");
            }
            catch (Exception ex)
            {
                await ShowMessage("Ошибка", $"Не удалось открыть проект: {ex.Message}");
            }
        }
        public async Task SaveAsync()
        {
            try
            {
                if (_fileService == null)
                {
                    await ShowMessage("Ошибка", "FileService не инициализирован");
                    return;
                }

                string? filePath = _currentFilePath;

                // Если это новый проект или Save As
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = await _fileService.SaveFileAsync("VecEditor Project|*.json", "project.json");
                    if (string.IsNullOrEmpty(filePath))
                        return;
                }

                var project = new ProjectDocument
                {
                    Figures = ConvertPrimitivesToFigures(),
                    LastModified = DateTime.Now
                };

                var json = _serializer.Serialize(project);
                await File.WriteAllTextAsync(filePath, json);

                _currentFilePath = filePath;

                await ShowMessage("Успех", $"Проект сохранен в: {filePath}");
            }
            catch (Exception ex)
            {
                await ShowMessage("Ошибка", $"Не удалось сохранить проект: {ex.Message}");
            }
        }
        public async Task SaveAsAsync()
        {
            var oldPath = _currentFilePath;
            _currentFilePath = null; // Временный сброс, чтобы SaveAsync показал диалог
            await SaveAsync();

            if (string.IsNullOrEmpty(_currentFilePath))
            {
                _currentFilePath = oldPath; // Восстанавливаем если отмена
            }
        }
        private List<IFigure> ConvertPrimitivesToFigures()
        {
            var figures = new List<IFigure>();

            foreach (var primitive in PrimitiveObjects)
            {
                var figure = ConvertPrimitiveToFigure(primitive);
                if (figure != null)
                {
                    figures.Add(figure);
                }
            }

            return figures;
        }
        public async Task ExportToPngAsync()
        {
            try
            {
                if (_fileService == null)
                {
                    await ShowMessage("Ошибка", "FileService не инициализирован");
                    return;
                }

                var path = await _fileService.SaveFileAsync("PNG Image|*.png", "image.png");
                if (string.IsNullOrEmpty(path))
                    return;

                var exporter = new PngExporter();
                var figures = ConvertPrimitivesToFigures();

                using var stream = File.Create(path);
                exporter.Export(figures, stream);

                await ShowMessage("Успех", $"Экспортировано в PNG: {path}");
            }
            catch (Exception ex)
            {
                await ShowMessage("Ошибка", $"Не удалось экспортировать в PNG: {ex.Message}");
            }
        }

        // Экспорт в SVG
        public async Task ExportToSvgAsync()
        {
            try
            {
                if (_fileService == null)
                {
                    await ShowMessage("Ошибка", "FileService не инициализирован");
                    return;
                }

                var path = await _fileService.SaveFileAsync("SVG Image|*.svg", "image.svg");
                if (string.IsNullOrEmpty(path))
                    return;

                var exporter = new SvgExporter();
                var figures = ConvertPrimitivesToFigures();

                using var stream = File.Create(path);
                exporter.Export(figures, stream);

                await ShowMessage("Успех", $"Экспортировано в SVG: {path}");
            }
            catch (Exception ex)
            {
                await ShowMessage("Ошибка", $"Не удалось экспортировать в SVG: {ex.Message}");
            }
        }
        private IFigure? ConvertPrimitiveToFigure(PrimitiveObjectState primitive)
        {
         
            if (primitive.PrimitiveType == "Line" && primitive.ObjectPoints.Count >= 2)
            {
                var p1 = primitive.ObjectPoints[0];
                var p2 = primitive.ObjectPoints[1];

                var pointA = new Point2(p1.X, p1.Y);
                var pointB = new Point2(p2.X, p2.Y);

                return new LineFigure(pointA, pointB);
            }

            // TODO: Добавить конвертацию для Rectangle, Ellipse, Triangle и других фигур

            return null;
        }

        private PrimitiveObjectState? ConvertFigureToPrimitive(IFigure figure)
        {
            
            if (figure is LineFigure line)
            {
                var points = new List<Avalonia.Point>
        {
            new Avalonia.Point(line.A.X, line.A.Y),
            new Avalonia.Point(line.B.X, line.B.Y)
        };

                return new PrimitiveObjectState(
                    MainWindowViewModel.PrimitiveType.Line,
                    MainWindowViewModel.ToolType.Pen,
                    points,
                    SelectedStrokeColor,
                    SelectedStrokeThickness
                );
            }

            // TODO: Добавить конвертацию для других фигур

            return null;
        }

        // Событие для показа сообщений
        public event EventHandler<(string Title, string Message)>? ShowMessageRequested;

        private async Task ShowMessage(string title, string message)
        {
            ShowMessageRequested?.Invoke(this, (title, message));
        }
        public async Task NewProjectAsync()
        {
            PrimitiveObjects.Clear();
            _currentFilePath = null;
            this.RaisePropertyChanged(nameof(PrimitiveObjects));
            historyManager.Clear();

            ShowMessage("Новый проект", "Создан новый проект");
        }
        public void ZoomIn()
        {
            Zoom += 0.1;
        }

        public void ZoomOut()
        {
            Zoom -= 0.1;
        }

        public void ZoomReset()
        {
            Zoom = 1.0;
        }
    }
}
