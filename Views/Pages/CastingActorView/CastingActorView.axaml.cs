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
public partial class CastingActorView : ReactiveUserControl<CastingActorViewModel>
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

    public CastingActorView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // bind to grid
            this.Bind(ViewModel, vm => vm.CastingActors,
                view => view.CastingActorsGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<CastingActor>)
                .DisposeWith(disposables);

            // binds
            this.Bind(ViewModel, vm => vm.SelectedIdx,
                view => view.CastingActorsGrid.SelectedIndex).DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Name, view => view.NameBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Surname, view => view.SurnameBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Patronymic, view => view.PatronymicBox.Text)
                .DisposeWith(disposables);

            // validation binds
            this.BindValidation(ViewModel, vm => vm.Name, view => view.NameBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Surname, view => view.SurnameBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Patronymic, view => view.PatronymicBoxValidation.Text)
                .DisposeWith(disposables);

            // bind commands
            this.BindCommand(ViewModel, vm => vm.AddCastingActor, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateCastingActor, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.DeleteCastingActor, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
    => IsFormShow = !IsFormShow;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (0 <= ViewModel.SelectedIdx && ViewModel.SelectedIdx < ViewModel.CastingActors.Count)
        {
            ViewModel.Name = ViewModel.CastingActors[ViewModel.SelectedIdx].Name;
            ViewModel.Surname = ViewModel.CastingActors[ViewModel.SelectedIdx].Surname;
            ViewModel.Patronymic = ViewModel.CastingActors[ViewModel.SelectedIdx].Patronymic;
        }
    }
}