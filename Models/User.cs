using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class User : INotifyPropertyChanged
{
    [Key]
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}