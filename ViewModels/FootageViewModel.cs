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

public class FootageViewModel : ViewModelBase
{
    // arrays
    public ObservableCollection<Footage> Footages { get; set; }
    public ObservableCollection<Movie> Movies { get; set; }
    public Array Statuses { get; set; } = Enum.GetValues(typeof(FootageStatus));

    // indexes
    [Reactive] public int SelectedIdx { get; set; }
    [Reactive] public int SelectedMovieIdx { get; set; }
    [Reactive] public int SelectedStatusIdx { get; set; }

    // attrubutes
    [Reactive] public string SceneName { get; set; }
    [Reactive] public TimeSpan TimeSpan { get; set; }
    [Reactive] public int TakeCount { get; set; }

    public ReactiveCommand<Unit, Unit> AddFootage { get; set; }
    public ReactiveCommand<Unit, Unit> UpdateFootage { get; set; }
    public ReactiveCommand<Unit, Unit> RemoveFootage { get; set; }

    private Movie SelectedMovie => Movies[SelectedMovieIdx];
    private FootageStatus SelectedStatus
        => (FootageStatus)Statuses.GetValue(SelectedStatusIdx)!;
    private Footage SelectedFootage => Footages[SelectedIdx];

    public FootageViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        Footages = new(
            db.Footages
                .Include(e => e.Movie)
                // .ThenInclude(e => e.)
                .ToList()
        );
        Movies = new(db.Movies);

        // TODO: validation

        AddFootage = ReactiveCommand.Create(_addFootage, this.IsValid());
        UpdateFootage = ReactiveCommand.Create(_updateFootage, this.IsValid());
        RemoveFootage = ReactiveCommand.Create(_removeFootage);
    }

    private async void _addFootage()
    {
        var footage = new Footage()
        {
            SceneName = SceneName,
            TimeSpan = TimeSpan,
            TakeCount = TakeCount,
            Status = SelectedStatus,
            Movie = SelectedMovie
        };

        await db.Footages.AddAsync(footage);
        await db.SaveChangesAsync();
        Footages.Add(footage);
    }

    private void _updateFootage()
    {
        SelectedFootage.SceneName = SceneName;
        SelectedFootage.TimeSpan = TimeSpan;
        SelectedFootage.TakeCount = TakeCount;
        SelectedFootage.Status = SelectedStatus;
        SelectedFootage.Movie = SelectedMovie;

        var list = db.Footages.ToList();



        footage.SceneName = SceneName;
        footage.TimeSpan = TimeSpan;
        footage.TakeCount = TakeCount;
        footage.Status = SelectedStatus;
        footage.Movie = SelectedMovie;

        db.SaveChanges();
    }

    private async void _removeFootage()
    {
        db.Footages.Remove(SelectedFootage);
        await db.SaveChangesAsync();
        Footages.RemoveAt(SelectedIdx);
    }
}