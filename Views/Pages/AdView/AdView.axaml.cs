using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Reactive.Disposables;
using System.Linq;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using System.Collections.Generic;
using FilmStudio.Models;
using DynamicData;
using System.Collections.ObjectModel;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class AdView : ReactiveUserControl<AdViewModel>
{
    private bool _isAdFormShow = false;
    public bool IsAdFormShow
    {
        get => _isAdFormShow;
        set
        {
            _isAdFormShow = value;
            this.AddAdForm.IsVisible = _isAdFormShow;
        }
    }

    public AdView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.SelectedMovieIdx,
                view => view.MovieComboBox.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedAdTypeIdx,
                view => view.AdTypeComboBox.SelectedIndex)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.Source, view => view.AdSourceBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Amount, view => view.AdAmountNumeric.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.TargetAudience,
                view => view.AdTargetAudienceBox.Text).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Movies, view => view.MovieComboBox.Items,
                vmToViewConverter: value => value,
                viewToVmConverter: value => value as ObservableCollection<Movie>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.AdTypes, view => view.AdTypeComboBox.Items,
                vmToViewConverter: value => value,
                viewToVmConverter: value => value as ObservableCollection<AdType>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Ads, view => view.AdsGrid.Items,
                vmToViewConverter: value => value,
                viewToVmConverter: view => view as ObservableCollection<Ad>)
                .DisposeWith(disposables);

            this.BindValidation(ViewModel, vm => vm.Source,
                view => view.AdSourceBoxValidation.Text).DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Amount,
                view => view.AdAmountNumericValidation.Text).DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.TargetAudience,
                view => view.AdTargetAudienceBoxValidation.Text).DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Movies,
                view => view.MovieComboBoxValidation.Text).DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.AdTypes,
                view => view.AdTypeComboBoxValidation.Text).DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.AddAd, view => view.AddAdButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateAd, view => view.UpdateAdButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveAd, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
     => IsAdFormShow = !IsAdFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (0 <= ViewModel.SelectedAdIdx && ViewModel.SelectedAdIdx < ViewModel.Ads.Count)
        {
            ViewModel!.Source = ViewModel.Ads[AdsGrid.SelectedIndex].Source;
            ViewModel.Amount = ViewModel.Ads[AdsGrid.SelectedIndex].Amount;
            ViewModel.TargetAudience = ViewModel.Ads[AdsGrid.SelectedIndex].TargetAudience;

            if (0 <= ViewModel.SelectedMovieIdx && ViewModel.SelectedMovieIdx < ViewModel.Movies.Count)
                ViewModel.SelectedMovieIdx = ViewModel.Movies.IndexOf(
                    ViewModel.Movies.ElementAt(ViewModel.SelectedMovieIdx)
                );

            if (0 <= ViewModel.SelectedAdTypeIdx && ViewModel.SelectedAdTypeIdx < ViewModel.AdTypes.Count)

                ViewModel.SelectedAdTypeIdx = ViewModel.AdTypes.IndexOf(
                    ViewModel.AdTypes.ElementAt(ViewModel.SelectedAdTypeIdx)
                );
        }
    }
}