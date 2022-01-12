using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmStudio.Models;

public class Movie
{
    [Key]
    public int MovieId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReleaseYear { get; set; }
    public virtual Employee Employee { get; set; }
    public MovieStatus Status { get; set; }
}

public enum MovieStatus
{
    Concept,
    Prepare,
    InProgress,
    Filming,
    Editing,
    Promo,
    InCinema
}