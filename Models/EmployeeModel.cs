using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    [Column(TypeName = "Date")]
    public DateTime BirthDate { get; set; }
    public string PassportNumber { get; set; }
    public string SNILS { get; set; }
    public string INN { get; set; }
    public virtual ICollection<Movie> Movies { get; set; }
}