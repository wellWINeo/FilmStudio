using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class AdType : INotifyPropertyChanged
{
    [Key]
    public int AdTypeId { get; set; }
    public string Name { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}