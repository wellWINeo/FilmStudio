using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class AdType
{
    [Key]
    public int AdTypeId { get; set; }
    public string Name { get; set; }
}