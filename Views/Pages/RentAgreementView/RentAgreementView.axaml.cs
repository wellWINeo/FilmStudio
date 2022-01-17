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
public partial class RentAgreementView : ReactiveUserControl<RentAgreementViewModel>
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

    public RentAgreementView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // bind sources
            this.Bind(ViewModel, vm => vm.RentAgreements, view => view.RentAgreementsGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<RentAgreement>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Movies, view => view.MoviesComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Movie>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Cinemas, view => view.CinemasComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Cinema>)
                .DisposeWith(disposables);

            // bind attrubutes
            this.Bind(ViewModel, vm => vm.RentStartDate, view => view.RentStartDate.SelectedDate)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.RentEndDate, view => view.RentEndDate.SelectedDate)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Amount, view => view.AmountNumeric.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedCinemaIdx,
                view => view.CinemasComboBox.SelectedIndex).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedMovieIdx,
                view => view.MoviesComboBox.SelectedIndex).DisposeWith(disposables);

            // bind validation
            this.BindValidation(ViewModel, vm => vm.RentStartDate,
                view => view.RentStartDateValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.RentEndDate,
                 view => view.RentEndDateValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Amount,
                view => view.AmountNumericValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.SelectedCinemaIdx,
                view => view.CinemasComboBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.SelectedMovieIdx,
                view => view.MoviesComboBoxValidation.Text)
                .DisposeWith(disposables);

            // bind commands
            this.BindCommand(ViewModel, vm => vm.AddRentAgreement, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateRentAgreement, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveRentAgreement, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
        => IsFormShow = !IsFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // ViewModel!.Source = ViewModel.Ads[AdsGrid.SelectedIndex].Source;
        // ViewModel.Amount = ViewModel.Ads[AdsGrid.SelectedIndex].Amount;
        // ViewModel.TargetAudience = ViewModel.Ads[AdsGrid.SelectedIndex].TargetAudience;
        // ViewModel.SelectedMovieIdx = ViewModel.Movies.IndexOf(
        //     ViewModel.Movies.ElementAt(ViewModel.SelectedMovieIdx)
        // );
        // ViewModel.SelectedAdTypeIdx = ViewModel.AdTypes.IndexOf(
        //     ViewModel.AdTypes.ElementAt(ViewModel.SelectedAdTypeIdx)
        // );
    }
}