using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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
    [Reactive] public DateTimeOffset? RentStartDate { get; set; } = DateTimeOffset.Now;
    [Reactive] public DateTimeOffset? RentEndDate { get; set; } = DateTimeOffset.Now;
    [Reactive] public decimal Amount { get; set; }
    [Reactive] public Cinema Cinema { get; set; }
    [Reactive] public Movie Movie { get; set; }

    // commands to interact with entity
    public ReactiveCommand<Unit, Unit> AddRentAgreement { get; }
    public ReactiveCommand<Unit, Unit> UpdateRentAgreement { get; }
    public ReactiveCommand<Unit, Unit> RemoveRentAgreement { get; }

    public RentAgreementViewModel(IScreen screen) : base(screen)
    {
        // loading agreements with eager loading
        RentAgreements = new(db.RentAgreements
            .Include(e => e.Cinema)
            .Include(e => e.Movie)
            .ToList());
        Movies = new(db.Movies);
        Cinemas = new(db.Cinemas);

        // validation
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

        // init commands
        AddRentAgreement = ReactiveCommand.Create(_addRentAgreement, this.IsValid());
        UpdateRentAgreement = ReactiveCommand.Create(_updateRentAgreement, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedIdx, x => 0 <= x && x < RentAgreements.Count),
            (x, y) => x && y
        // checks is data valid & agreement selected in grid for update
        ));
        RemoveRentAgreement = ReactiveCommand.Create(_removeRentAgreement, this.WhenAnyValue(
            x => x.SelectedIdx, x => 0 <= x && x < RentAgreements.Count // is index valid
        ));
    }

    private void _addRentAgreement()
    {
        var rentAgreement = new RentAgreement()
        {
            RentStartDate = _getDateTimeFromOffset(RentStartDate),
            RentEndDate = _getDateTimeFromOffset(RentEndDate),
            Amount = Amount,
            Cinema = SelectedCinema,
            Movie = SelectedMovie
        };

        db.RentAgreements.Add(rentAgreement);
        db.SaveChanges();
        RentAgreements.Add(rentAgreement);
    }

    private void _removeRentAgreement()
    {
        db.RentAgreements.Remove(RentAgreements[SelectedIdx]);
        db.SaveChanges();
        RentAgreements.RemoveAt(SelectedIdx);
    }

    private void _updateRentAgreement()
    {
        RentAgreements[SelectedIdx].RentStartDate = _getDateTimeFromOffset(RentStartDate);
        RentAgreements[SelectedIdx].RentEndDate = _getDateTimeFromOffset(RentEndDate);
        RentAgreements[SelectedIdx].Amount = Amount;
        RentAgreements[SelectedIdx].Cinema = SelectedCinema;
        RentAgreements[SelectedIdx].Movie = SelectedMovie;

        db.RentAgreements.Update(RentAgreements[SelectedIdx]);
        db.SaveChanges();
    }

    private DateTime _getDateTimeFromOffset(DateTimeOffset? value)
        => ((DateTimeOffset)value!).DateTime;
}