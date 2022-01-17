using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmStudio.Models;

public class FilmSet : INotifyPropertyChanged
{
    [Key]
    public int FilmSetId { get; set; }
    public string Location { get; set; }
    public virtual Movie Movie { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}