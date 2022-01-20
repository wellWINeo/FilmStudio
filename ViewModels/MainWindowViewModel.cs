using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FilmStudio.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    private ApplicationContext db;

    public LoginViewModel loginViewModel { get; set; }

    public ReactiveCommand<Unit, IRoutableViewModel> ChangeView { get; }

    public RoutingState Router { get; } = new();

    [Reactive] public int CurrentIndex { get; set; } = 0;

    public Array ListOfSubsystems { get; set; } = Enum.GetValues(typeof(Subsystem));


    public MainWindowViewModel()
    {
        db = new();
        loginViewModel = new(this);
        ChangeView = ReactiveCommand.CreateFromObservable(_changeView, this.WhenAnyValue(
            x => x.CurrentIndex,
            x => 0 <= x && x < ListOfSubsystems.Length
        ));
    }


    private IObservable<IRoutableViewModel> _changeView()
        => ListOfSubsystems.GetValue(CurrentIndex) switch
        {
            Subsystem.Employees => Router.Navigate.Execute(
                new EmployeeViewModel(this)
            ),

            Subsystem.CastingList => Router.Navigate.Execute(
                new CastingListViewModel(this)
            ),

            Subsystem.FilmSets => Router.Navigate.Execute(
                new FilmSetViewModel(this)
            ),

            Subsystem.Footages => Router.Navigate.Execute(
                new FootageViewModel(this)
            ),

            Subsystem.Movies => Router.Navigate.Execute(
                new MovieViewModel(this)
            ),

            Subsystem.Cinemas => Router.Navigate.Execute(
                new CinemasViewModel(this)
            ),

            Subsystem.Ad => Router.Navigate.Execute(
                new AdViewModel(this)
            ),

            Subsystem.AdType => Router.Navigate.Execute(
                new AdTypeViewModel(this)
            ),

            Subsystem.RentAgreement => Router.Navigate.Execute(
                new RentAgreementViewModel(this)
            ),

            Subsystem.CastingActor => Router.Navigate.Execute(
                new CastingActorViewModel(this)
            ),

            Subsystem.Position => Router.Navigate.Execute(
                new PositionViewModel(this)
            ),

            Subsystem.Users => Router.Navigate.Execute(
                new UserViewModel(this)
            ),

            Subsystem.Props => Router.Navigate.Execute(
                new PropsViewModel(this)
            ),

            _ => Router.Navigate.Execute(
                new ViewModelBase(this)
            ),
        };
}
