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
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.loginViewModel.IsNotAuthed, view => view.LocalLoginView.IsVisible)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.loginViewModel.IsNotAuthed, view => view.MainTabControl.IsVisible,
                    value => !value, value => !value)
                    .DisposeWith(disposables);
            });
#if DEBUG
            this.AttachDevTools();
#endif

        }
    }
}