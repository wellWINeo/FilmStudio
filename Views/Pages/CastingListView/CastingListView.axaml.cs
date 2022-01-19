using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using FilmStudio.ViewModels;
using Avalonia.Interactivity;
using PropertyChanged;
using System.Collections.ObjectModel;
using FilmStudio.Models;
using System.Linq;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class CastingListView : ReactiveUserControl<CastingListViewModel>
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

    public CastingListView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // bind sources
            this.Bind(ViewModel, vm => vm.Movies, view => view.MovieComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Movie>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.CastingActors, view => view.CastingActorComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<CastingActor>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.CastingLists, view => view.CastingListGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<CastingList>)
                .DisposeWith(disposables);

            // bind
            this.Bind(ViewModel, vm => vm.Role, view => view.RoleBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.AtDateTime, view => view.DatePicker.SelectedDate)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedIdx, view => view.CastingListGrid.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedActorIdx,
                view => view.CastingActorComboBox.SelectedIndex).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedMovieIdx,
                view => view.MovieComboBox.SelectedIndex).DisposeWith(disposables);

            // bind validation
            this.BindValidation(ViewModel, vm => vm.Role, view => view.RoleBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.AtDateTime, view => view.DatePickerValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.SelectedActorIdx,
                view => view.CastingActorComboBoxValidation.Text).DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.SelectedMovieIdx,
                view => view.MovieComboBoxValidation.Text).DisposeWith(disposables);

            // bind commands
            this.BindCommand(ViewModel, vm => vm.AddToCastingList, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateInCastingList, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveFromCastingList, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
        => IsFormShow = !IsFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (0 <= ViewModel.SelectedIdx && ViewModel.SelectedIdx < ViewModel.CastingLists.Count)
        {
            ViewModel.Role = ViewModel.CastingLists[ViewModel.SelectedIdx].Role;
            ViewModel.AtDateTime = ViewModel.CastingLists[ViewModel.SelectedIdx].Datetime;
            // TODO: wtf is it ?!
            ViewModel.SelectedActorIdx = ViewModel.CastingActors.IndexOf(
                ViewModel.CastingActors.ElementAt(ViewModel.SelectedActorIdx)
            );
            ViewModel.SelectedMovieIdx = ViewModel.Movies.IndexOf(
                ViewModel.Movies.ElementAt(ViewModel.SelectedMovieIdx)
            );
        }
    }
}