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
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class PositionView : ReactiveUserControl<PositionViewModel>
{
    public PositionView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // bind source
            this.Bind(ViewModel, vm => vm.Positions, view => view.PositionsGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<Position>)
                .DisposeWith(disposables);

            // bind
            this.Bind(ViewModel, vm => vm.SelectedIdx,
                view => view.PositionsGrid.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Title, view => view.NameBox.Text)
                .DisposeWith(disposables);

            // bind validation
            this.BindValidation(ViewModel, vm => vm.Title,
                view => view.NameBoxValidation.Text)
                .DisposeWith(disposables);

            // bind commands
            this.BindCommand(ViewModel, vm => vm.AddPosition, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdatePosition, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemovePosition, view => view.DeleteButton)
                .DisposeWith(disposables);

        });
    }

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (0 <= ViewModel.SelectedIdx && ViewModel.SelectedIdx < ViewModel.Positions.Count)
            ViewModel.Title = ViewModel.Positions[PositionsGrid.SelectedIndex].Title;
    }
}
