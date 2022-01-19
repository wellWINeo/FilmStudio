using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class CastingActorViewModel : ViewModelBase
{
    public ObservableCollection<CastingActor> CastingActors { get; set; }

    [Reactive] public int SelectedIdx { get; set; }

    [Reactive] public string Name { get; set; } = string.Empty;
    [Reactive] public string Surname { get; set; } = string.Empty;
    [Reactive] public string Patronymic { get; set; } = "-";

    public ReactiveCommand<Unit, Unit> AddCastingActor { get; }
    public ReactiveCommand<Unit, Unit> UpdateCastingActor { get; }
    public ReactiveCommand<Unit, Unit> DeleteCastingActor { get; }


    public CastingActorViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        CastingActors = new(db.CastingActors);

        this.ValidationRule(
            vm => vm.Name,
            value => !string.IsNullOrWhiteSpace(value),
            "Name can't be empty"
        );
        this.ValidationRule(
            vm => vm.Surname,
            value => !string.IsNullOrWhiteSpace(value),
            "Surname can't be empty"
        );

        this.ValidationRule(
            vm => vm.Patronymic,
            value => !string.IsNullOrWhiteSpace(value),
            "-"
        );

        AddCastingActor = ReactiveCommand.Create(_addCastingActor, this.IsValid());
        UpdateCastingActor = ReactiveCommand.Create(_updateRentAgreement, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedIdx, x => 0 <= x && x < CastingActors.Count),
            (x, y) => x && y
        ));
        DeleteCastingActor = ReactiveCommand.Create(_removeRentAgreement, this.WhenAnyValue(
            x => x.SelectedIdx, x => 0 <= x && x < CastingActors.Count
        ));
    }

    private void _addCastingActor()
    {
        var actor = new CastingActor()
        {
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
        };

        db.CastingActors.Add(actor);
        db.SaveChanges();
        CastingActors.Add(actor);
    }

    private void _removeRentAgreement()
    {
        db.CastingActors.Remove(CastingActors[SelectedIdx]);
        db.SaveChanges();
        CastingActors.RemoveAt(SelectedIdx);
    }

    private void _updateRentAgreement()
    {
        CastingActors[SelectedIdx].Name = Name;
        CastingActors[SelectedIdx].Surname = Surname;
        CastingActors[SelectedIdx].Patronymic = Patronymic;

        db.CastingActors.Update(CastingActors[SelectedIdx]);
        db.SaveChanges();
    }
}