using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FilmStudio.Models;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;
using System;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class FootageView : ReactiveUserControl<FootageViewModel>
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

    public FootageView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // bind source
            this.Bind(ViewModel, vm => vm.Footages, view => view.FootagesGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Footage>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Movies, view => view.MovieComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Movie>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Statuses, view => view.StatusComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as Array)
                .DisposeWith(disposables);

            // binds
            this.Bind(ViewModel, vm => vm.SceneName, view => view.SceneBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.TimeSpan, view => view.TimeSpanPicker.SelectedTime)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.TakeCount, view => view.TakeCountNumeric.Value)
                .DisposeWith(disposables);

            // index binding
            this.Bind(ViewModel, vm => vm.SelectedIdx, view => view.FootagesGrid.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedMovieIdx, view => view.MovieComboBox.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedStatusIdx, view => view.StatusComboBox.SelectedIndex)
                .DisposeWith(disposables);

            // commands
            this.BindCommand(ViewModel, vm => vm.AddFootage, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateFootage, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveFootage, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
    => IsFormShow = !IsFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SceneName = ViewModel.Footages[ViewModel.SelectedIdx].SceneName;
        ViewModel.TimeSpan = ViewModel.Footages[ViewModel.SelectedIdx].TimeSpan;
        ViewModel.TakeCount = ViewModel.Footages[ViewModel.SelectedIdx].TakeCount;

        // TODO: what the fuck!!!
        ViewModel.SelectedMovieIdx = ViewModel.Movies.IndexOf(
            ViewModel.Movies.ElementAt(ViewModel.SelectedMovieIdx)
        );
        // ViewModel.SelectedStatusIdx = ViewModel.Statuses.IndexOf(
        //     ViewModel.Statuses.ElementAt(ViewModel.SelectedStatusIdx)
        // );
    }
}