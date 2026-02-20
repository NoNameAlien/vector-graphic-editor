using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using VecEditor.Core.Figures;
using vector_graphic_editor.ViewModels;
using vector_graphic_editor.Views;

namespace vector_graphic_editor
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();

            var line = new LineFigure(new (0,0),new(1,1));
            var param = new Newtonsoft.Json.JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            var res=Newtonsoft.Json.JsonConvert.SerializeObject(line,param);

            var copy= Newtonsoft.Json.JsonConvert.DeserializeObject<IFigure>(res,param);
            Console.WriteLine(res);

        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}
