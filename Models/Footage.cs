using System;

namespace FilmStudio.Models;

public class Footage
{
    public int FootageId { get; set; }
    public string SceneName { get; set; }
    public TimeSpan TimeSpan { get; set; }
    public uint TakeCount { get; set; }
    public FootageStatus Status { get; set; }
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