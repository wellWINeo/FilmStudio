using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Reactive.Disposables;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;
using System.Collections.ObjectModel;
using FilmStudio.Models;
using System;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class MovieView : ReactiveUserControl<MovieViewModel>
{

    private bool _isMovieFormShow = false;
    public bool IsMovieFormShow
    {
        get => _isMovieFormShow;
        set
        {
            _isMovieFormShow = value;
            this.AddMovieForm.IsVisible = _isMovieFormShow;
        }
    }

    public MovieView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Movies, view => view.MoviesGrid.Items,
                vmToViewConverter: value => value,
                viewToVmConverter: value => value as ObservableCollection<Movie>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.ListOfStatuses, view => view.MovieStatus.Items,
                vmToViewConverter: value => value,
                viewToVmConverter: value => value as Array)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.SelectedMovieIndex,
                view => view.MoviesGrid.SelectedIndex).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedStatusIndex,
                view => view.MovieStatus.SelectedIndex).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.Title, view => view.MovieTitleBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Description,
                view => view.MovieDescriptionBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.ReleaseYear, view => view.MovieReleaseYear.Text)
                .DisposeWith(disposables);

            this.BindValidation(ViewModel, vm => vm.Title, view => view.MovieTitleBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Description,
                view => view.MovieDescriptionBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.ReleaseYear,
                view => view.MovieReleaseYearValidation.Text)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.AddMovie, view => view.AddMovieButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateMovie, view => view.UpdateMovieButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveMovie, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
        => IsMovieFormShow = !IsMovieFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (0 <= ViewModel.SelectedMovieIndex && ViewModel.SelectedMovieIndex < ViewModel.Movies.Count)
        {
            ViewModel.Title = ViewModel.Movies[MoviesGrid.SelectedIndex].Title;
            ViewModel.Description = ViewModel.Movies[MoviesGrid.SelectedIndex].Description;
            ViewModel.ReleaseYear = ViewModel.Movies[MoviesGrid.SelectedIndex].ReleaseYear;
        }
    }
}