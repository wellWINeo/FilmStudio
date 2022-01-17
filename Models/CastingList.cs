using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class CastingList : INotifyPropertyChanged
{
    [Key]
    public int CastingListId { get; set; }

    public string Role { get; set; }
    public DateTime Datetime { get; set; }
    public int IdMovie { get; set; }
    public int IdCastingActor { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}