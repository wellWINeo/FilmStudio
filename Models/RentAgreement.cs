using System;
using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class RentAgreement
{
    [Key]
    public int RentAgreementId { get; set; }
    public DateOnly RentStartDate { get; set; }
    public DateOnly RentEndDate { get; set; }
    public decimal Amount { get; set; }
    public virtual Cinema Cinema { get; set; }
    public virtual Movie Movie { get; set; }
}