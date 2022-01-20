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

    public FilmSetViewModel(IScreen screen) : base(screen)
    {
        // loading film sets with eager loading for movie
        FilmSets = new(db.FilmSets
            .Include(e => e.Movie)
            .ToList()
        );

        // all movies
        Movies = new(db.Movies);

        // validation
        this.ValidationRule(
            vm => vm.Location,
            value => !string.IsNullOrWhiteSpace(value),
            "Location can't be empty"
        );

        this.ValidationRule(
            vm => vm.SelectedMovieIdx,
            value => 0 <= value && value < Movies.Count,
            "Select movie!"
        );

        // init commands
        AddFilmSet = ReactiveCommand.Create(_addFilmSet, this.IsValid());
        UpdateFilmSet = ReactiveCommand.Create(_updateFilmSet, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedIdx, x => 0 <= x && x < FilmSets.Count),
            (x, y) => x && y
        ));
        RemoveFilmSet = ReactiveCommand.Create(_removeFilmSet, this.WhenAnyValue(
            x => x.SelectedIdx, x => 0 <= x && x < FilmSets.Count
        ));
    }

    private void _addFilmSet()
    {
        var filmSet = new FilmSet()
        {
            Location = Location,
            Movie = SelectedMovie()
        };

        db.FilmSets.Add(filmSet);
        db.SaveChanges();
        FilmSets.Add(filmSet);
    }

    private void _updateFilmSet()
    {
        SelectedFilmSet().Location = Location;
        SelectedFilmSet().Movie = SelectedMovie();

        db.FilmSets.Update(SelectedFilmSet());
        db.SaveChanges();
    }

    private void _removeFilmSet()
    {
        db.FilmSets.Remove(SelectedFilmSet());
        db.SaveChanges();
        FilmSets.RemoveAt(SelectedIdx);
    }
}