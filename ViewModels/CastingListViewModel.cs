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

public class CastingListViewModel : ViewModelBase
{
    public ObservableCollection<CastingList> CastingLists { get; set; }
    public ObservableCollection<Movie> Movies { get; set; }
    public ObservableCollection<CastingActor> CastingActors { get; set; }

    [Reactive] public string Role { get; set; }
    [Reactive] public DateTimeOffset? AtDateTime { get; set; } = DateTimeOffset.Now;

    [Reactive] public int SelectedMovieIdx { get; set; }
    [Reactive] public int SelectedActorIdx { get; set; }
    [Reactive] public int SelectedIdx { get; set; }

    public ReactiveCommand<Unit, Unit> AddToCastingList { get; }
    public ReactiveCommand<Unit, Unit> UpdateInCastingList { get; }
    public ReactiveCommand<Unit, Unit> RemoveFromCastingList { get; }

    private Movie SelectedMovie => Movies[SelectedMovieIdx];
    private CastingActor SelectedCastingActor => CastingActors[SelectedActorIdx];
    private CastingList SelectedCastingList => CastingLists[SelectedIdx];

    public CastingListViewModel(ApplicationContext _db, IScreen screen) :
        base(_db, screen)
    {

        CastingLists = new(
            db.CastingLists
            .Include(e => e.Movie)
            .Include(e => e.CastingActor)
            .ToList()
        );
        Movies = new(db.Movies);
        CastingActors = new(db.CastingActors);

        // validation
        this.ValidationRule(
            vm => vm.Role,
            value => !string.IsNullOrWhiteSpace(value),
            "Role can't be empty!"
        );

        this.ValidationRule(
            vm => vm.AtDateTime,
            value => value != null,
            "Select date!"
        );

        this.ValidationRule(
            vm => vm.SelectedMovieIdx,
            value => 0 <= value && value < Movies.Count,
            "Select movie!"
        );

        this.ValidationRule(
            vm => vm.SelectedActorIdx,
            value => 0 <= value && value < CastingActors.Count,
            "Select actor to cast!"
        );

        AddToCastingList = ReactiveCommand.Create(_addToCastingList, this.IsValid());
        UpdateInCastingList = ReactiveCommand.Create(_updateInCastingList, this.IsValid());
        RemoveFromCastingList = ReactiveCommand.Create(_removeFromCastingList, this.WhenAnyValue(
            x => x.SelectedIdx, x => 0 <= x && x < CastingLists.Count
        ));

    }

    private async void _addToCastingList()
    {
        var casting = new CastingList()
        {
            Role = Role,
            Datetime = _getDateTimeFromOffset(AtDateTime),
            Movie = SelectedMovie,
            CastingActor = SelectedCastingActor
        };

        await db.CastingLists.AddAsync(casting);
        await db.SaveChangesAsync();
        CastingLists.Add(casting);
    }

    private async void _removeFromCastingList()
    {
        db.CastingLists.Remove(SelectedCastingList);
        await db.SaveChangesAsync();
        CastingLists.RemoveAt(SelectedIdx);
    }

    private async void _updateInCastingList()
    {
        SelectedCastingList.Role = Role;
        SelectedCastingList.Datetime = _getDateTimeFromOffset(AtDateTime);
        SelectedCastingList.Movie = SelectedMovie;
        SelectedCastingList.CastingActor = SelectedCastingActor;

        db.CastingLists.Update(SelectedCastingList);
        await db.SaveChangesAsync();
    }

    private DateTime _getDateTimeFromOffset(DateTimeOffset? offset)
        => ((DateTimeOffset)offset!).DateTime;
}
