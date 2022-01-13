using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using FilmStudio.ViewModels;

namespace FilmStudio.Views
{
    public partial class EmployeeView : ReactiveUserControl<EmployeeViewModel>
    {
        public EmployeeView()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.Name, view => view.EmployeeNameBox.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Surname, view => view.EmployeeSurnameBox.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Patronymic, view => view.EmployeePatronymicBox)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Salary, view => view.EmployeeSalaryBox)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.BirthDate, view => view.EmployeeBirthDatePickerirthDate)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.PassportNumber, view => view.EmployeePassportNumberBox.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SNILS, view => view.EmployeeSNILSBox.Text)
                    .DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.INN, view => view.EmployeeINNBox.Text)
                    .DisposeWith(disposables);

                this.BindValidation(ViewModel, vm => vm.Name, view => view.EmployeeNameBoxValidation.Text)
                    .DisposeWith(disposables);
                this.BindValidation(ViewModel, vm => vm.Surname, view => view.EmployeeSurnameBoxValidation.Text)
                    .DisposeWith(disposables);
                this.BindValidation(ViewModel, vm => vm.Patronymic, view => view.EmployeePatronymicBox)
                    .DisposeWith(disposables);
            });
        }
    }
}