using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using VecEditor.App.ViewModels;
using VecEditor.IO;
using VecEditor.IO.Services;

namespace VecEditor.App.Views;

public partial class MainWindow : Window
{
    private readonly FileService _fileService;
    private readonly JsonProjectSerializer _serializer;

    public MainWindow()
    {
        InitializeComponent();

        _fileService = new FileService(this);
        _serializer = new JsonProjectSerializer();
        DataContext = new MainWindowViewModel();

        var fileService = new FileService(this);
        // Настраиваем ViewModel
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.SetFileService(_fileService);
            viewModel.ShowMessageRequested += async (s, args) =>
            {
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await ShowMessage(args.Title, args.Message);
                });
            };
        }

        this.KeyDown += OnKeyDown;
        // Подключаем обработчики меню
        //var newMenuItem = this.FindControl<MenuItem>("NewMenuItem");
        //if (newMenuItem != null)
        //    newMenuItem.Click += async (s, e) =>
        //    {
        //        if (DataContext is MainWindowViewModel vm)
        //            await vm.NewProjectAsync();
        //    };

        //var openMenuItem = this.FindControl<MenuItem>("OpenMenuItem");
        //if (openMenuItem != null)
        //    openMenuItem.Click += async (s, e) =>
        //    {
        //        if (DataContext is MainWindowViewModel vm)
        //            await vm.OpenAsync();
        //    };

        //var saveMenuItem = this.FindControl<MenuItem>("SaveMenuItem");
        //if (saveMenuItem != null)
        //    saveMenuItem.Click += async (s, e) =>
        //    {
        //        if (DataContext is MainWindowViewModel vm)
        //            await vm.SaveAsync();
        //    };

        //var saveAsMenuItem = this.FindControl<MenuItem>("SaveAsMenuItem");
        //if (saveAsMenuItem != null)
        //    saveAsMenuItem.Click += async (s, e) =>
        //    {
        //        if (DataContext is MainWindowViewModel vm)
        //            await vm.SaveAsAsync();
        //    };

        //var exportPngMenuItem = this.FindControl<MenuItem>("ExportPngMenuItem");
        //if (exportPngMenuItem != null)
        //    exportPngMenuItem.Click += async (s, e) =>
        //    {
        //        if (DataContext is MainWindowViewModel vm)
        //            await vm.ExportToPngAsync();
        //    };

        //var exportSvgMenuItem = this.FindControl<MenuItem>("ExportSvgMenuItem");
        //if (exportSvgMenuItem != null)
        //    exportSvgMenuItem.Click += async (s, e) =>
        //    {
        //        if (DataContext is MainWindowViewModel vm)
        //            await vm.ExportToSvgAsync();
        //    };

        //var exitMenuItem = this.FindControl<MenuItem>("ExitMenuItem");
        //if (exitMenuItem != null)
        //    exitMenuItem.Click += (s, e) => Close();
    }

    private async void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            if (e.KeyModifiers == KeyModifiers.Control)
            {
                // Проверяем нажатие Ctrl
                switch (e.Key)
                {
                    case Key.N:
                        await viewModel.NewProjectAsync();
                        e.Handled = true;
                        break;
                    case Key.O:
                        await viewModel.OpenAsync();
                        e.Handled = true;
                        break;
                    case Key.S:
                        await viewModel.SaveAsync();
                        e.Handled = true;
                        break;
                    case Key.Add:
                    case Key.OemPlus:
                        viewModel.ZoomIn();
                        e.Handled = true;
                        break;
                    case Key.Subtract:
                    case Key.OemMinus:
                        viewModel.ZoomOut();
                        e.Handled = true;
                        break;
                }
            }
        }
    }

    private async Task ShowMessage(string title, string message)
    {
        try
        {
            var msgWindow = new Window
            {
                Title = title,
                Width = 300,
                Height = 150,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var stackPanel = new StackPanel
            {
                Margin = new Thickness(20),
                Children =
            {
                new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 20),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                },
                new Button
                {
                    Content = "OK",
                    Width = 80,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                }
            }
            };

            msgWindow.Content = stackPanel;

            var okButton = (Button)stackPanel.Children[1];
            okButton.Click += (s, e) => msgWindow.Close();

            await msgWindow.ShowDialog(this);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ShowMessage error: {ex.Message}");
        }
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            var point = e.GetPosition((Visual?)sender);

            if (viewModel.SelectedTool == MainWindowViewModel.ToolType.Pointer)
            {
                viewModel.SelectObjectAt(point);
            }
            else if (viewModel.SelectedTool == MainWindowViewModel.ToolType.Eraser)
            {
                viewModel.DeleteObjectAt(point);
            }
            else
            {
                viewModel.AddPoint(point);
            }
        }
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            var point = e.GetPosition((Visual?)sender);
            viewModel.UpdatePreview(point);
        }
    }

    private void StrokeThicknessTextBox_GotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.CaretIndex = textBox.Text?.Length ?? 0;
            textBox.SelectionStart = textBox.CaretIndex;
            textBox.SelectionEnd = textBox.CaretIndex;
        }
    }
}