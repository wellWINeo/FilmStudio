using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using FilmStudio.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class FilmSetViewModel : ViewModelBase
{
    public ObservableCollection<FilmSet> FilmSets { get; set; }
    public ObservableCollection<Movie> Movies { get; set; }

    [Reactive] public string Location { get; set; }

    [Reactive] public int SelectedIdx { get; set; }
    [Reactive] public int SelectedMovieIdx { get; set; }

    public ReactiveCommand<Unit, Unit> AddFilmSet { get; }
    public ReactiveCommand<Unit, Unit> UpdateFilmSet { get; }
    public ReactiveCommand<Unit, Unit> RemoveFilmSet { get; }

    private Movie SelectedMovie() => Movies[SelectedMovieIdx];
    private FilmSet SelectedFilmSet() => FilmSets[SelectedIdx];

    public FilmSetViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        FilmSets = new(db.FilmSets
            .Include(e => e.Movie)
            .ToList()
        );

        Movies = new(db.Movies);

        // TODO: validation

        AddFilmSet = ReactiveCommand.Create(_addFilmSet, this.IsValid());
        UpdateFilmSet = ReactiveCommand.Create(_updateFilmSet, this.IsValid());
        RemoveFilmSet = ReactiveCommand.Create(_removeFilmSet);
    }

    private async void _addFilmSet()
    {
        var filmSet = new FilmSet()
        {
            Location = Location,
            Movie = SelectedMovie()
        };

        await db.FilmSets.AddAsync(filmSet);
        await db.SaveChangesAsync();
        FilmSets.Add(filmSet);
    }

    private void _updateFilmSet()
    {
        SelectedFilmSet().Location = Location;
        SelectedFilmSet().Movie = SelectedMovie();

        db.FilmSets.Update(SelectedFilmSet());
        db.SaveChanges();
    }

    private async void _removeFilmSet()
    {
        db.FilmSets.Remove(SelectedFilmSet());
        await db.SaveChangesAsync();
        FilmSets.RemoveAt(SelectedIdx);
    }
}