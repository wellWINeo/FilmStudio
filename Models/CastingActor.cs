using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class CastingActor : INotifyPropertyChanged
{
    [Key]
    public int CastingActorId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}