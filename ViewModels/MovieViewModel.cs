using System;
using System.Collections.ObjectModel;
using System.Reactive;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class MovieViewModel : ViewModelBase
{
    public ObservableCollection<Movie> Movies { get; set; }
    public Array ListOfStatuses { get; set; } = Enum.GetValues(typeof(MovieStatus));

    [Reactive] public int SelectedMovieIndex { get; set; }

    [Reactive] public string Title { get; set; }
    [Reactive] public string Description { get; set; }
    [Reactive] public int ReleaseYear { get; set; }
    [Reactive] public int SelectedStatusIndex { get; set; }

    public ReactiveCommand<Unit, Unit> AddMovie { get; }
    public ReactiveCommand<Unit, Unit> UpdateMovie { get; }
    public ReactiveCommand<Unit, Unit> RemoveMovie { get; }


    public MovieViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        Movies = new(db.Movies);

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

        AddMovie = ReactiveCommand.Create(_addMovie, this.IsValid());
        UpdateMovie = ReactiveCommand.Create(_updateMovie, this.IsValid());
        RemoveMovie = ReactiveCommand.Create(_removeMovie, this.WhenAnyValue(
            x => x.SelectedMovieIndex, x => 0 <= x && x < Movies.Count
        ));

    }

    private async void _addMovie()
    {
        var movie = new Movie()
        {
            Title = Title,
            Description = Description,
            ReleaseYear = ReleaseYear,
            Status = _castMovieStatus()
        };

        await db.Movies.AddAsync(movie);
        await db.SaveChangesAsync();
        Movies.Add(movie);
    }

    private async void _removeMovie()
    {
        if (0 <= SelectedStatusIndex && SelectedStatusIndex <= Movies.Count)
        {
            db.Movies.Remove(Movies[SelectedMovieIndex]);
            await db.SaveChangesAsync();
            Movies.RemoveAt(SelectedMovieIndex);
        }
    }

    private async void _updateMovie()
    {
        Movies[SelectedMovieIndex].Title = Title;
        Movies[SelectedMovieIndex].Description = Description;
        Movies[SelectedMovieIndex].ReleaseYear = ReleaseYear;
        Movies[SelectedMovieIndex].Status = _castMovieStatus();

        db.Movies.Update(Movies[SelectedMovieIndex]);
        await db.SaveChangesAsync();
    }

    private MovieStatus _castMovieStatus()
        => (MovieStatus)ListOfStatuses.GetValue(SelectedStatusIndex);
}