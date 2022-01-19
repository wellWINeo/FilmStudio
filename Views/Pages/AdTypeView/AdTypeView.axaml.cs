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
public partial class AdTypeView : ReactiveUserControl<AdTypeViewModel>
{
    public AdTypeView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.SelectedAdIndex, view => view.AdTypeGrid.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.AdTypes, view => view.AdTypeGrid.Items,
                vmToViewConverter: value => value,
                viewToVmConverter: value => value as ObservableCollection<AdType>)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.Name, view => view.AdTypeNameBox.Text)
                .DisposeWith(disposables);

            this.BindValidation(ViewModel, vm => vm.Name,
                view => view.AdTypeNameBoxValidation.Text)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.AddAdType, view => view.AddAdTypeButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateAdType, view => view.UpdateAdTypeButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveAdType, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (0 <= AdTypeGrid.SelectedIndex && AdTypeGrid.SelectedIndex < ViewModel.AdTypes.Count)
            ViewModel.Name = ViewModel.AdTypes[AdTypeGrid.SelectedIndex].Name;
    }

}