using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using FilmStudio.ViewModels;
using FilmStudio.Views;
using ReactiveUI;
using Splat;

namespace FilmStudio;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        // locator registration
        Locator.CurrentMutable.Register(() => new EmployeeView(),
            typeof(IViewFor<EmployeeViewModel>));

        Locator.CurrentMutable.Register(() => new CastingListView(),
            typeof(IViewFor<CastingListViewModel>));

        Locator.CurrentMutable.Register(() => new CircleView(),
            typeof(IViewFor<ViewModelBase>));

        Locator.CurrentMutable.Register(() => new AdTypeView(),
            typeof(IViewFor<AdTypeViewModel>));

        // build app
        return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}