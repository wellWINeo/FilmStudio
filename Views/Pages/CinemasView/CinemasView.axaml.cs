using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FilmStudio.Models;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class CinemasView : ReactiveUserControl<CinemasViewModel>
{
    private bool _isCinemaFormShow = false;
    public bool IsCinemaFormShow
    {
        get => _isCinemaFormShow;
        set
        {
            _isCinemaFormShow = value;
            this.AddCinemaForm.IsVisible = _isCinemaFormShow;
        }
    }

    public CinemasView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Cinemas, view => view.CinemasGrid.Items,
                vmToViewConverter: value => value,
                viewToVmConverter: value => value as ObservableCollection<Cinema>)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.SelectedCinemaIndex,
                view => view.CinemasGrid.SelectedIndex).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Title, view => view.CinemaTitleBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Address, view => view.CinemaAddressBox.Text)
                .DisposeWith(disposables);

            this.BindValidation(ViewModel, vm => vm.Title, view => view.CinemaTitleBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Address, view => view.CinemaAddressBoxValidation.Text)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.AddCinema, view => view.AddCinemaButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateCinema, view => view.UpdateCinemaButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveCinema, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
    => IsCinemaFormShow = !IsCinemaFormShow;


    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.Title = ViewModel.Cinemas[CinemasGrid.SelectedIndex].Title;
        ViewModel.Address = ViewModel.Cinemas[CinemasGrid.SelectedIndex].Address;

    }
}