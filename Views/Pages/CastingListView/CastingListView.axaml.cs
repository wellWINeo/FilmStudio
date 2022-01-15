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

namespace FilmStudio.Views
{
    [DoNotNotify]
    public partial class CastingListView : ReactiveUserControl<CastingListViewModel>
    {
        public CastingListView()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.CastingLists, view => view.CastingListGrid.Items,
                    vmToViewConverterOverride: new EmployeesConverter(),
                    viewToVMConverterOverride: new EmployeesConverter())
                    .DisposeWith(disposables);
            });
        }
    }
}