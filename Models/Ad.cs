using System.ComponentModel.DataAnnotations;

namespace FilmStudio.Models;

public class Ad
{
    [Key]
    public int AdId { get; set; }
    public string Source { get; set; }
    public decimal Amount { get; set; }
    public string TargetAudience { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual AdType AdType { get; set; }
}