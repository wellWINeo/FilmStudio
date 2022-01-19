using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

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

        // validation
        this.ValidationRule(
            vm => vm.Title,
            value => !string.IsNullOrWhiteSpace(value),
            "Title can't be empty"
        );

        AddPosition = ReactiveCommand.Create(_addPosition, this.IsValid());
        UpdatePosition = ReactiveCommand.Create(_updatePosition, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedIdx, x => 0 <= x && x < Positions.Count),
            (x, y) => x && y
        ));
        RemovePosition = ReactiveCommand.Create(_removePosition, this.WhenAnyValue(
            x => x.SelectedIdx, x => 0 <= x && x < Positions.Count
        ));
    }

    private void _addPosition()
    {
        var position = new Position() { Title = Title };
        db.Positions.Add(position);
        db.SaveChanges();
        Positions.Add(position);
    }

    private void _removePosition()
    {
        db.Positions.Remove(Positions[SelectedIdx]);
        db.SaveChanges();
        Positions.RemoveAt(SelectedIdx);
    }

    private void _updatePosition()
    {
        if (SelectedIdx >= 0 && Positions[SelectedIdx].Title != Title)
        {
            Positions[SelectedIdx].Title = Title;
            db.Positions.Update(Positions[SelectedIdx]);
            db.SaveChanges();
        }
    }
}