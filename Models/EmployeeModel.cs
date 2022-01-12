using System;
using System.ComponentModel.DataAnnotations;
using ReactiveUI;

namespace FilmStudio.Models;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public decimal Salary { get; set; }
    public DateOnly BirthDate { get; set; }
    public string PassportNumber { get; set; }
    public string SNILS { get; set; }
    public string INN { get; set; }
}