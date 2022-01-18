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
public partial class UserView : ReactiveUserControl<UserViewModel>
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

    public UserView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            // bind
            this.Bind(ViewModel, vm => vm.SelectedIdx, view => view.UsersGrid.SelectedIndex)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Users, view => view.UsersGrid.Items,
                vmToViewConverter: v => v,
                viewToVmConverter: v => v as ObservableCollection<User>)
                .DisposeWith(disposables);

            // bind attributes
            this.Bind(ViewModel, vm => vm.UserName, view => view.UserNameBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Password, view => view.PasswordBox.Text)
                .DisposeWith(disposables);

            // bind validation
            this.BindValidation(ViewModel, vm => vm.UserName,
                view => view.UserNameBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Password,
                view => view.PasswordBoxValidation.Text)
                .DisposeWith(disposables);

            // bind commands
            this.BindCommand(ViewModel, vm => vm.AddUser, view => view.AddButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateUser, view => view.UpdateButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.RemoveUser, view => view.DeleteButton)
                .DisposeWith(disposables);
        });
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
        => IsFormShow = !IsFormShow;


    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.UserName = ViewModel.Users[ViewModel.SelectedIdx].UserName;
        ViewModel.Password = ViewModel.Users[ViewModel.SelectedIdx].Password;
    }
}