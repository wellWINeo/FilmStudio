using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Reactive.Disposables;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;
using System;
using FilmStudio.Models;

namespace FilmStudio.Views;

[DoNotNotify]
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{

    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.Router, view => view.ViewHost.Router)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.loginViewModel.IsNotAuthed,
                view => view.LocalLoginView.IsVisible).DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.loginViewModel.IsNotAuthed,
                view => view.MainPanel.IsVisible,
                value => !value, value => !value)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.CurrentIndex,
                view => view.SubsystemList.SelectedIndex)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.ListOfSubsystems,
                    view => view.SubsystemList.Items).DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.ChangeView, view => view.GoSubsystemButton)
                .DisposeWith(disposables);
        });
#if DEBUG
        this.AttachDevTools();
#endif

    }
}