using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class Position
{
    [Key]
    public int PositionId { get; set; }
    public string Title { get; set; }
}