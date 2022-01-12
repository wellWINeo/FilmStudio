using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class Cinema
{
    [Key]
    public int CinemasId { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
}