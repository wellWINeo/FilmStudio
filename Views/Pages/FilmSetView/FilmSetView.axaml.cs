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
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class FilmSetView : ReactiveUserControl<FilmSetViewModel>
{

    private bool _isFormShow = false;
    public bool IsFormShow
    {
        get => _isFormShow;
        set
        {
            _isFormShow = value;
            this.AddForm.IsVisible = _isFormShow;
        }
    }

    public FilmSetView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // bind sources
            this.Bind(ViewModel, vm => vm.FilmSets, view => view.FilmSetsGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<FilmSet>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Movies, view => view.MovieComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Movie>)
                .DisposeWith(disposables);

            // index bind
            this.Bind(ViewModel, vm => vm.SelectedIdx,
                view => view.FilmSetsGrid.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedMovieIdx,
                view => view.MovieComboBox.SelectedIndex)
                .DisposeWith(disposables);

            // bind
            this.Bind(ViewModel, vm => vm.Location, view => view.LocationBox.Text)
                .DisposeWith(disposables);

            // bind validation
            this.BindValidation(ViewModel, vm => vm.Location,
                view => view.LocationBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.SelectedMovieIdx,
                view => view.MovieComboBoxValidation.Text)
                .DisposeWith(disposables);

            // command
            this.BindCommand(ViewModel, vm => vm.AddFilmSet, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateFilmSet, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveFilmSet, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
        => IsFormShow = !IsFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel!.Location = ViewModel.FilmSets[ViewModel.SelectedIdx].Location;
    }
}