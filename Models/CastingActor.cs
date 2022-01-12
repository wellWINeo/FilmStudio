using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class CastingActor
{
    [Key]
    public int CastingActorId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }

}