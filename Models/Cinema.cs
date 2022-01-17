using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class Cinema : INotifyPropertyChanged
{
    [Key]
    public int CinemaId { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}