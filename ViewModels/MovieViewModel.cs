using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class MovieViewModel : ViewModelBase
{
    // sources
    public ObservableCollection<Movie> Movies { get; set; }
    public Array ListOfStatuses { get; set; } = Enum.GetValues(typeof(MovieStatus));

    [Reactive] public int SelectedMovieIndex { get; set; }

    // attributes
    [Reactive] public string Title { get; set; }
    [Reactive] public string Description { get; set; }
    [Reactive] public int ReleaseYear { get; set; }
    [Reactive] public int SelectedStatusIndex { get; set; }

    public ReactiveCommand<Unit, Unit> AddMovie { get; }
    public ReactiveCommand<Unit, Unit> UpdateMovie { get; }
    public ReactiveCommand<Unit, Unit> RemoveMovie { get; }


    public MovieViewModel(IScreen screen) : base(screen)
    {
        Movies = new(db.Movies);

        // validation
        this.ValidationRule(
            vm => vm.Title,
            title => !string.IsNullOrWhiteSpace(title),
            "Title can't be empty"
        );

        this.ValidationRule(
            vm => vm.Description,
            description => !string.IsNullOrWhiteSpace(description),
            "Description can't be empty"
        );

        this.ValidationRule(
            vm => vm.ReleaseYear,
            year => 0 < year,
            "Year must greater than 0"
        );

        // commands
        AddMovie = ReactiveCommand.Create(_addMovie, this.IsValid());
        UpdateMovie = ReactiveCommand.Create(_updateMovie, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedMovieIndex, x => 0 <= x && x < Movies.Count),
            (x, y) => x && y
        ));
        RemoveMovie = ReactiveCommand.Create(_removeMovie, this.WhenAnyValue(
            x => x.SelectedMovieIndex, x => 0 <= x && x < Movies.Count
        ));

    }

    private void _addMovie()
    {
        var movie = new Movie()
        {
            Title = Title,
            Description = Description,
            ReleaseYear = ReleaseYear,
            Status = _castMovieStatus()
        };

        db.Movies.Add(movie);
        db.SaveChanges();
        Movies.Add(movie);
    }

    private void _removeMovie()
    {
        if (0 <= SelectedStatusIndex && SelectedStatusIndex <= Movies.Count)
        {
            db.Movies.Remove(Movies[SelectedMovieIndex]);
            db.SaveChanges();
            Movies.RemoveAt(SelectedMovieIndex);
        }
    }

    private void _updateMovie()
    {
        Movies[SelectedMovieIndex].Title = Title;
        Movies[SelectedMovieIndex].Description = Description;
        Movies[SelectedMovieIndex].ReleaseYear = ReleaseYear;
        Movies[SelectedMovieIndex].Status = _castMovieStatus();

        db.Movies.Update(Movies[SelectedMovieIndex]);
        db.SaveChanges();
    }

    private MovieStatus _castMovieStatus()
        => (MovieStatus)ListOfStatuses.GetValue(SelectedStatusIndex);
}