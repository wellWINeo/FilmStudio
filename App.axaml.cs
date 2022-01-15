using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FilmStudio.ViewModels;
using FilmStudio.Views;
using PropertyChanged;

namespace FilmStudio;

[DoNotNotify]
public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var db = new ApplicationContext();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {

                DataContext = new MainWindowViewModel(new EmployeeViewModel(db),
                                                    new CastingListViewModel(db),
                                                    new LoginViewModel(db)),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}