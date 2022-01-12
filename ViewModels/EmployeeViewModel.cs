using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FilmStudio.Models;
using ReactiveUI;

namespace FilmStudio.ViewModels;

public class EmployeeViewModel : ViewModelBase
{
    private ApplicationContext db;
    public ObservableCollection<Employee> Employees { get; set; }

    public EmployeeViewModel()
    {
        db = new ApplicationContext();
        Employees = new ObservableCollection<Employee>()
        {
            new Employee {Name="Name1", Surname="Surname1", Patronymic="Patronymic1"},
            new Employee {Name="Name2", Surname="Surname2", Patronymic="Patronymic2"},
            new Employee {Name="Name3", Surname="Surname3", Patronymic="Patronymic3"},
        };
    }
}