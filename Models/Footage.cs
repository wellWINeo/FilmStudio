using System;
using System.ComponentModel;

namespace FilmStudio.Models;

public class Footage : INotifyPropertyChanged
{
    public int FootageId { get; set; }
    public string SceneName { get; set; }
    public TimeSpan TimeSpan { get; set; }
    public int TakeCount { get; set; }
    public FootageStatus Status { get; set; }
    public virtual Movie Movie { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
}

public enum FootageStatus
{
    Planned,
    InProgress,
    Filmed,
    Editing,
    Archived,
    UsedInMovie
}