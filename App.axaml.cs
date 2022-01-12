using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FilmStudio.ViewModels;
using FilmStudio.Views;

namespace FilmStudio
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(new EmployeeViewModel(),
                                                        new CastingListViewModel()),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}