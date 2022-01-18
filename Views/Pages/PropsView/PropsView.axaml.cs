using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Reactive.Disposables;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;
using System.Collections.ObjectModel;
using FilmStudio.Models;
using Avalonia.Interactivity;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class PropsView : ReactiveUserControl<PropsViewModel>
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

    public PropsView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {

            // sources bind
            this.Bind(ViewModel, vm => vm.Props, view => view.PropsGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Props>)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.FilmSets, view => view.FilmSetComboBox.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<FilmSet>)
                .DisposeWith(disposables);

            // bind indexes
            this.Bind(ViewModel, vm => vm.SelectedIdx, view => view.PropsGrid.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SelectedFilmSetIdx,
                view => view.FilmSetComboBox.SelectedIndex)
                .DisposeWith(disposables);

            // attribute binds
            this.Bind(ViewModel, vm => vm.Title, view => view.TitleBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Description, view => view.DescriptionBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Qunatity, view => view.QuantityNumeric.Value)
                .DisposeWith(disposables);

            // bind commands
            this.BindCommand(ViewModel, vm => vm.AddProps, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateProps, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveProps, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
        => IsFormShow = !IsFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.Title = ViewModel.Props[PropsGrid.SelectedIndex].Title;
        ViewModel.Description = ViewModel.Props[PropsGrid.SelectedIndex].Description;
        ViewModel.Qunatity = ViewModel.Props[PropsGrid.SelectedIndex].Quantity;
    }
}