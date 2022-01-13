using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Data;
using FilmStudio.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Validation.States;
using ReactiveUI.Fody.Helpers;
using System.Text.RegularExpressions;

namespace FilmStudio.ViewModels;

public class EmployeeViewModel : ReactiveValidationObject
{
    private ApplicationContext db;

    private bool isAddUserMode = false;
    public bool IsAddUserMode
    {
        get => isAddUserMode;
        set => this.RaiseAndSetIfChanged(ref isAddUserMode, value);
    }

    public IEnumerable<Employee> Employees { get; set; }

    public ReactiveCommand<Unit, Unit> EnterAddUserMode { get; }

    // Employee attributes
    // private string _name;
    [Reactive]
    public string Name { get; set; } = string.Empty;

    // private string _surname;
    [Reactive]
    public string Surname { get; set; } = string.Empty;

    public string Patronymic { get; set; }
    public decimal Salary { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    public string PassportNumber { get; set; }
    public string SNILS { get; set; }
    public string INN { get; set; }

    public EmployeeViewModel(ApplicationContext _db)
    {
        db = _db;
        Employees = db.Employees.AsEnumerable();
        // Employees = new ObservableCollection<Employee>()
        // {
        //     new Employee {Name="Name1", Surname="Surname1", Patronymic="Patronymic1"},
        //     new Employee {Name="Name2", Surname="Surname2", Patronymic="Patronymic2"},
        //     new Employee {Name="Name3", Surname="Surname3", Patronymic="Patronymic3"},
        // };

        // configure validation
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
        EnterAddUserMode = ReactiveCommand.Create(enterAddUserMode);
    }

    private async void enterAddUserMode()
    {
        if (IsAddUserMode)
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
        }

        // Show/hide add form
        IsAddUserMode = !IsAddUserMode;
    }

}