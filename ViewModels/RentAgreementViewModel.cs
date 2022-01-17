using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using FilmStudio.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class RentAgreementViewModel : ViewModelBase
{
    // collections for grid & comboboxes
    public ObservableCollection<RentAgreement> RentAgreements { get; set; }
    public ObservableCollection<Movie> Movies { get; set; }
    public ObservableCollection<Cinema> Cinemas { get; set; }

    // INPC vars to interact with user selection
    [Reactive] public int SelectedMovieIdx { get; set; }
    [Reactive] public int SelectedCinemaIdx { get; set; }
    [Reactive] public int SelectedIdx { get; set; }
    // private
    private Cinema SelectedCinema => Cinemas[SelectedCinemaIdx];

    private Movie SelectedMovie => Movies[SelectedMovieIdx];

    // entity attributes
    [Reactive] public DateTimeOffset RentStartDate { get; set; }
    [Reactive] public DateTimeOffset RentEndDate { get; set; }
    [Reactive] public decimal Amount { get; set; }
    [Reactive] public Cinema Cinema { get; set; }
    [Reactive] public Movie Movie { get; set; }

    // commands to interact with entity
    public ReactiveCommand<Unit, Unit> AddRentAgreement { get; }
    public ReactiveCommand<Unit, Unit> UpdateRentAgreement { get; }
    public ReactiveCommand<Unit, Unit> RemoveRentAgreement { get; }

    public RentAgreementViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        RentAgreements = new(db.RentAgreements
            .Include(e => e.Cinema)
            .Include(e => e.Movie)
            .ToList());
        Movies = new(db.Movies);
        Cinemas = new(db.Cinemas);

        this.ValidationRule(
            vm => vm.RentStartDate,
            value => value != null,
            "Start date of rent can't be empty"
        );

        this.ValidationRule(
            vm => vm.RentEndDate,
            value => RentEndDate >= RentStartDate,
            "End date can't be before start of rent"
        );

        this.ValidationRule(
            vm => vm.Amount,
            value => value >= 0,
            "Amount can't be negative"
        );

        this.ValidationRule(
            vm => vm.SelectedCinemaIdx,
            value => Cinemas.Count() >= SelectedCinemaIdx,
            "Choose cinema!"
        );

        this.ValidationRule(
            vm => vm.SelectedMovieIdx,
            value => Movies.Count() >= SelectedMovieIdx,
            "Choose movie!"
        );

        AddRentAgreement = ReactiveCommand.Create(_addRentAgreement, this.IsValid());
        RemoveRentAgreement = ReactiveCommand.Create(_updateRentAgreement, this.IsValid());
        UpdateRentAgreement = ReactiveCommand.Create(_removeRentAgreement);
    }

    private async void _addRentAgreement()
    {
        var rentAgreement = new RentAgreement()
        {
            RentStartDate = RentStartDate.DateTime,
            RentEndDate = RentEndDate.DateTime,
            Amount = Amount,
            Cinema = SelectedCinema,
            Movie = SelectedMovie
        };

        await db.RentAgreements.AddAsync(rentAgreement);
        await db.SaveChangesAsync();
        RentAgreements.Add(rentAgreement);
    }

    private async void _removeRentAgreement()
    {
        db.RentAgreements.Remove(RentAgreements[SelectedIdx]);
        await db.SaveChangesAsync();
        RentAgreements.RemoveAt(SelectedIdx);
    }

    private async void _updateRentAgreement()
    {
        RentAgreements[SelectedIdx].RentStartDate = RentStartDate.DateTime;
        RentAgreements[SelectedIdx].RentEndDate = RentEndDate.DateTime;
        RentAgreements[SelectedIdx].Amount = Amount;
        RentAgreements[SelectedIdx].Cinema = SelectedCinema;
        RentAgreements[SelectedIdx].Movie = SelectedMovie;

        db.RentAgreements.Update(RentAgreements[SelectedIdx]);
        await db.SaveChangesAsync();
    }
}