using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Fody.Helpers;
using System.Text.RegularExpressions;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;

namespace FilmStudio.ViewModels;

public class EmployeeViewModel : ViewModelBase
{
    // Commands
    public ReactiveCommand<Unit, Unit> AddUserCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateUserCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteUserCommand { get; }

    public ObservableCollection<Employee> Employees { get; set; }

    [Reactive] public int EmployeeSelectedIndex { get; set; }

    public IObservable<bool> IsEmployeeSelected { get; set; }

    // protperties for Employee model
    [Reactive] public string Name { get; set; } = string.Empty;
    [Reactive] public string Surname { get; set; } = string.Empty;
    [Reactive] public string Patronymic { get; set; } = "-";
    [Reactive] public decimal Salary { get; set; } = 0.0M;
    [Reactive] public DateTimeOffset? BirthDate { get; set; } = DateTimeOffset.Now;
    [Reactive] public string PassportNumber { get; set; } = string.Empty;
    [Reactive] public string SNILS { get; set; } = string.Empty;
    [Reactive] public string INN { get; set; } = string.Empty;

    public EmployeeViewModel(ApplicationContext _db, IScreen screen) :
        base(_db, screen)
    {
        Employees = new(db.Employees
            .Include(e => e.Movies)
            .ToList());

        IsEmployeeSelected = this.WhenAnyValue(
            x => x.EmployeeSelectedIndex,
            idx => 0 <= idx && idx < Employees.Count
        );

        // configure validation rules
        this.ValidationRule(
            vm => vm.Name,
            name => !string.IsNullOrWhiteSpace(name),
            "Name can't be empty!"
        );

        this.ValidationRule(
            vm => vm.Surname,
            surname => !string.IsNullOrWhiteSpace(surname),
            "Surname can't be empty!"
        );

        this.ValidationRule(
            vm => vm.Patronymic,
            patronymic => !string.IsNullOrWhiteSpace(patronymic),
            "-" // default value
        );

        this.ValidationRule(
            vm => vm.Salary,
            salary => salary > 0,
            "Salary can't be negative!"
        );

        this.ValidationRule(
            vm => vm.BirthDate,
            birthDate => birthDate <= DateTimeOffset.Now,
            "Birth Date can't be in future!"
        );

        this.ValidationRule(
            vm => vm.PassportNumber,
            passportNumber => Regex.IsMatch(passportNumber ?? string.Empty, @"\d{4}-\d{6}"),
            "Passport number must satisfy format!"
        );

        this.ValidationRule(
            vm => vm.SNILS,
            snils => Regex.IsMatch(snils ?? string.Empty, @"\d{3}-\d{3}-\d{3} \d{2}"),
            "SNILS number must satisfy format!"
        );

        this.ValidationRule(
            vm => vm.INN,
            inn => Regex.IsMatch(inn ?? string.Empty, @"\d{12}"),
            "INN number must satisfy format!"
        );

        // init commands
        AddUserCommand = ReactiveCommand.Create(_addUserCommand, this.IsValid());
        UpdateUserCommand = ReactiveCommand.Create(_updateUserCommand, this.IsValid());
        DeleteUserCommand = ReactiveCommand.Create(_deleteUserCommand, IsEmployeeSelected);
    }

    private async void _addUserCommand()
    {
        var employee = new Employee
        {
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
            Salary = Salary,
            PassportNumber = PassportNumber,
            BirthDate = ((DateTimeOffset)BirthDate).DateTime,
            SNILS = SNILS,
            INN = INN
        };

        await db.Employees.AddAsync(employee);
        await db.SaveChangesAsync();
        Employees.Add(employee);
    }

    private void _updateUserCommand()
    {
        Employees[EmployeeSelectedIndex].Name = Name;
        Employees[EmployeeSelectedIndex].Surname = Surname;
        Employees[EmployeeSelectedIndex].Patronymic = Patronymic;
        Employees[EmployeeSelectedIndex].Salary = Salary;
        Employees[EmployeeSelectedIndex].PassportNumber = PassportNumber;
        Employees[EmployeeSelectedIndex].BirthDate = ((DateTimeOffset)BirthDate).DateTime;
        Employees[EmployeeSelectedIndex].SNILS = SNILS;
        Employees[EmployeeSelectedIndex].INN = INN;

        db.Employees.Update(Employees[EmployeeSelectedIndex]);
        db.SaveChanges();
    }

    private void _deleteUserCommand()
    {
        db.Employees.Remove(Employees[EmployeeSelectedIndex]);
        Employees.RemoveAt(EmployeeSelectedIndex);
        db.SaveChanges();
    }
}