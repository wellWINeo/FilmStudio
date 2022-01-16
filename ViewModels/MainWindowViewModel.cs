﻿using System;
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
    {
        // subsystem++;
        return ListOfSubsystems.GetValue(CurrentIndex) switch
        {
            Subsystem.Employees => Router.Navigate.Execute(
                new EmployeeViewModel(db, this)),

            Subsystem.CastingList => Router.Navigate.Execute(
                new CastingListViewModel(db, this)),

            Subsystem.FilmSets => Router.Navigate.Execute(
                new ViewModelBase(db, this)),

            Subsystem.Footages => Router.Navigate.Execute(
                new ViewModelBase(db, this)),

            Subsystem.Accounting => Router.Navigate.Execute(
                new ViewModelBase(db, this)),

            Subsystem.Movies => Router.Navigate.Execute(
                new ViewModelBase(db, this)),

            Subsystem.Cinemas => Router.Navigate.Execute(
                new ViewModelBase(db, this)),

            Subsystem.Ad => Router.Navigate.Execute(
                new ViewModelBase(db, this))
        };
    }
}
