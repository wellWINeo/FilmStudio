using System.Collections.ObjectModel;
using System.Reactive;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FilmStudio.ViewModels;

public class PositionViewModel : ViewModelBase
{
    public ObservableCollection<Position> Positions { get; set; }

    [Reactive] public int SelectedIdx { get; set; }
    [Reactive] public string Title { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> AddPosition { get; }
    public ReactiveCommand<Unit, Unit> UpdatePosition { get; }
    public ReactiveCommand<Unit, Unit> RemovePosition { get; }

    public PositionViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        Positions = new(db.Positions);

        // TODO: add validation

        AddPosition = ReactiveCommand.Create(_addPosition);
        UpdatePosition = ReactiveCommand.Create(_updatePosition);
        RemovePosition = ReactiveCommand.Create(_removePosition);
    }

    private async void _addPosition()
    {
        var position = new Position() { Title = Title };
        await db.Positions.AddAsync(position);
        await db.SaveChangesAsync();
        Positions.Add(position);
    }

    private async void _removePosition()
    {
        db.Positions.Remove(Positions[SelectedIdx]);
        await db.SaveChangesAsync();
        Positions.RemoveAt(SelectedIdx);
    }

    private async void _updatePosition()
    {
        if (SelectedIdx >= 0 && Positions[SelectedIdx].Title != Title)
        {
            Positions[SelectedIdx].Title = Title;
            db.Positions.Update(Positions[SelectedIdx]);
            await db.SaveChangesAsync();
        }
    }
}