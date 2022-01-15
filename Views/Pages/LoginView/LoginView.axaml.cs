using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using FilmStudio.ViewModels;
using Avalonia.Interactivity;
using FilmStudio.Helpers;
using PropertyChanged;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.UserName, view => view.LoginBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Password, view => view.PasswordBox.Text)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.Login, view => view.LoginButton)
                .DisposeWith(disposables);
        });
    }
}