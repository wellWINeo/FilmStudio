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
        loginViewModel = new(db, this);
        ChangeView = ReactiveCommand.CreateFromObservable(_changeView);
    }


    private IObservable<IRoutableViewModel> _changeView()
        => ListOfSubsystems.GetValue(CurrentIndex) switch
        {
            Subsystem.Employees => Router.Navigate.Execute(
                new EmployeeViewModel(db, this)
            ),

            Subsystem.CastingList => Router.Navigate.Execute(
                new CastingListViewModel(db, this)
            ),

            Subsystem.FilmSets => Router.Navigate.Execute(
                new FilmSetViewModel(db, this)
            ),

            Subsystem.Footages => Router.Navigate.Execute(
                new FootageViewModel(db, this)
            ),

            Subsystem.Movies => Router.Navigate.Execute(
                new MovieViewModel(db, this)
            ),

            Subsystem.Cinemas => Router.Navigate.Execute(
                new CinemasViewModel(db, this)
            ),

            Subsystem.Ad => Router.Navigate.Execute(
                new AdViewModel(db, this)
            ),

            Subsystem.AdType => Router.Navigate.Execute(
                new AdTypeViewModel(db, this)
            ),

            Subsystem.RentAgreement => Router.Navigate.Execute(
                new RentAgreementViewModel(db, this)
            ),

            Subsystem.CastingActor => Router.Navigate.Execute(
                new CastingActorViewModel(db, this)
            ),

            Subsystem.Position => Router.Navigate.Execute(
                new PositionViewModel(db, this)
            ),

            Subsystem.Users => Router.Navigate.Execute(
                new UserViewModel(db, this)
            ),

            Subsystem.Props => Router.Navigate.Execute(
                new PropsViewModel(db, this)
            )
        };
}
