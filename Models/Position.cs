using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class Position : INotifyPropertyChanged
{
    [Key]
    public int PositionId { get; set; }
    public string Title { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}