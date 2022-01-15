using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Reactive.Disposables;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;

namespace FilmStudio.Views
{
    [DoNotNotify]
    public partial class LoginView : ReactiveUserControl<LoginViewModel>
    {
        public LoginView()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.IsNotAuthed, view => view.IsVisible)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.UserName, view => view.LoginBox.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Password, view => view.PasswordBox.Text)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Login, view => view.LoginButton)
                    .DisposeWith(disposables);
            });
        }
    }
}