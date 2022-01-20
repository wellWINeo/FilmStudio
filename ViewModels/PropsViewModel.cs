using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using FilmStudio.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class PropsViewModel : ViewModelBase
{
    // sources
    public ObservableCollection<Props> Props { get; set; }
    public ObservableCollection<FilmSet> FilmSets { get; set; }

    // attributes
    [Reactive] public string Title { get; set; }
    [Reactive] public string Description { get; set; }
    [Reactive] public int Quantity { get; set; }

    // indexes
    [Reactive] public int SelectedIdx { get; set; }
    [Reactive] public int SelectedFilmSetIdx { get; set; }

    // commands
    public ReactiveCommand<Unit, Unit> AddProps { get; }
    public ReactiveCommand<Unit, Unit> UpdateProps { get; }
    public ReactiveCommand<Unit, Unit> RemoveProps { get; }

    private Props SelectedProps() => Props[SelectedIdx];
    private FilmSet SelectedFilmSet() => FilmSets[SelectedFilmSetIdx];

    public PropsViewModel(IScreen screen) : base(screen)
    {
        // loading props with eager loading for filmset and movie
        Props = new(db.Props
            .Include(e => e.FilmSet)
            .ThenInclude(e => e.Movie)
            .ToList()
        );

        // all film sets
        FilmSets = new(db.FilmSets.Include(e => e.Movie).ToList());

        // validation
        this.ValidationRule(
            vm => vm.Title,
            value => !string.IsNullOrWhiteSpace(value),
            "Title can't be empty!"
        );

        this.ValidationRule(
            vm => vm.Description,
            value => !string.IsNullOrWhiteSpace(value),
            "Description can't be empty!"
        );

        this.ValidationRule(
            vm => vm.Quantity,
            value => value >= 0,
            "Can't be negative!"
        );

        this.ValidationRule(
            vm => vm.SelectedFilmSetIdx,
            value => 0 <= value && value < FilmSets.Count,
            "Choose film set!"
        );

        // init commands
        AddProps = ReactiveCommand.Create(_addProps, this.IsValid());
        UpdateProps = ReactiveCommand.Create(_updateProps, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedIdx, x => 0 <= x && x < Props.Count),
            (x, y) => x && y
        ));
        RemoveProps = ReactiveCommand.Create(_removeProps, this.WhenAnyValue(
            x => x.SelectedIdx, x => 0 <= x && x < Props.Count
        ));
    }

    private void _addProps()
    {
        var prop = new Props()
        {
            Title = Title,
            Description = Description,
            Quantity = Quantity,
            FilmSet = SelectedFilmSet()
        };

        db.Props.Add(prop);
        db.SaveChanges();
        Props.Add(prop);
    }

    private void _updateProps()
    {
        SelectedProps().Title = Title;
        SelectedProps().Description = Description;
        SelectedProps().Quantity = Quantity;
        SelectedProps().FilmSet = SelectedFilmSet();

        db.Props.Update(SelectedProps());
        db.SaveChanges();
    }

    private void _removeProps()
    {
        db.Props.Remove(SelectedProps());
        db.SaveChanges();
        Props.RemoveAt(SelectedIdx);
    }
}