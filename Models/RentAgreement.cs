using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmStudio.Models;

public class RentAgreement : INotifyPropertyChanged
{
    [Key]
    public int RentAgreementId { get; set; }
    [Column(TypeName = "Date")]
    public DateTime RentStartDate { get; set; }
    [Column(TypeName = "Date")]
    public DateTime RentEndDate { get; set; }
    public decimal Amount { get; set; }
    public virtual Cinema Cinema { get; set; }
    public virtual Movie Movie { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}