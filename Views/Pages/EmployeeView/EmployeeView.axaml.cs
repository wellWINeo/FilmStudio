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
public partial class EmployeeView : ReactiveUserControl<EmployeeViewModel>
{
    private bool _isAddUserMode = false;
    public bool IsAddUserMode
    {
        get => _isAddUserMode;
        set
        {
            _isAddUserMode = value;
            this.AddUserForm.IsVisible = _isAddUserMode;
        }
    }

    public EmployeeView()
    {
        AvaloniaXamlLoader.Load(this);
        InitializeComponent();
        this.WhenActivated(disposables =>
        {

            // bind commands
            this.BindCommand(ViewModel, vm => vm.AddUserCommand, view => view.AddUserButton)
            .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.UpdateUserCommand, view => view.UpdateUserButton)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.DeleteUserCommand, view => view.DeleteButton)
                .DisposeWith(disposables);


            /* 
            // default binds
            */
            this.Bind(ViewModel, vm => vm.Employees, view => view.EmployeesGrid.Items,
                vmToViewConverterOverride: new EmployeesConverter(),
                viewToVMConverterOverride: new EmployeesConverter())
                .DisposeWith(disposables);
            // this.Bind(ViewModel, vm => vm.Employees, view => view.EmployeesGrid.Items)
            //     .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.EmployeeSelectedIndex, view => view.EmployeesGrid.SelectedIndex)
            .DisposeWith(disposables);

            // model binds
            this.Bind(ViewModel, vm => vm.Name, view => view.EmployeeNameBox.Text)
            .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Surname, view => view.EmployeeSurnameBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Patronymic, view => view.EmployeePatronymicBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.Salary, view => view.EmployeeSalaryBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.BirthDate, view => view.EmployeeBirthDatePicker.SelectedDate)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.PassportNumber, view => view.EmployeePassportNumberBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.SNILS, view => view.EmployeeSNILSBox.Text)
                .DisposeWith(disposables);
            this.Bind(ViewModel, vm => vm.INN, view => view.EmployeeINNBox.Text)
                .DisposeWith(disposables);

            // binds for validation error messages
            this.BindValidation(ViewModel, vm => vm.Name, view => view.EmployeeNameBoxValidation.Text)
            .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Surname, view => view.EmployeeSurnameBoxValidation.Text)
                .DisposeWith(disposables);
            // this.BindValidation(ViewModel, vm => vm.Patronymic, view => view.EmployeePatronymicBox.Text)
            //     .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.Salary, view => view.EmployeeSalaryBoxValidation.Text)
            .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.PassportNumber, view => view.EmployeePassportNumberBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.SNILS, view => view.EmployeeSNILSBoxValidation.Text)
                .DisposeWith(disposables);
            this.BindValidation(ViewModel, vm => vm.INN, view => view.EmployeeINNBoxValidation.Text)
                .DisposeWith(disposables);
        });
    }

    private void Startup()
    {
        AvaloniaXamlLoader.Load(this);
        this.AddUserForm.IsVisible = IsAddUserMode;
    }

    private void OnShowHideFormButtonClick(object sender, RoutedEventArgs e)
        => IsAddUserMode = !IsAddUserMode;

    private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (0 <= ViewModel.EmployeeSelectedIndex &&
            ViewModel.EmployeeSelectedIndex < ViewModel.Employees.Count)
        {
            ViewModel.Name = ViewModel.Employees[EmployeesGrid.SelectedIndex].Name;
            ViewModel.Surname = ViewModel.Employees[EmployeesGrid.SelectedIndex].Surname;
            ViewModel.Patronymic = ViewModel.Employees[EmployeesGrid.SelectedIndex].Patronymic;
            ViewModel.Salary = ViewModel.Employees[EmployeesGrid.SelectedIndex].Salary;
            ViewModel.BirthDate = ViewModel.Employees[EmployeesGrid.SelectedIndex].BirthDate;
            ViewModel.PassportNumber = ViewModel.Employees[EmployeesGrid.SelectedIndex].PassportNumber;
            ViewModel.SNILS = ViewModel.Employees[EmployeesGrid.SelectedIndex].SNILS;
            ViewModel.INN = ViewModel.Employees[EmployeesGrid.SelectedIndex].INN;
        }
    }

}