using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using PropertyChanged;
using ReactiveUI.Fody.Helpers;

namespace FilmStudio.Models;


public class Employee : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
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