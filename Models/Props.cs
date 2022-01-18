using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmStudio.Models;

public class Props : INotifyPropertyChanged
{
    [Key]
    public int PropsId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    // public string Location { get; set; }
    public virtual FilmSet FilmSet { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}