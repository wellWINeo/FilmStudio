using System;
using System.Collections.ObjectModel;
using System.Reactive;
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
        UpdateCastingActor = ReactiveCommand.Create(_updateRentAgreement, this.IsValid());
        DeleteCastingActor = ReactiveCommand.Create(_removeRentAgreement);
    }

    private async void _addCastingActor()
    {
        var actor = new CastingActor()
        {
            Name = Name,
            Surname = Surname,
            Patronymic = Patronymic,
        };

        await db.CastingActors.AddAsync(actor);
        await db.SaveChangesAsync();
        CastingActors.Add(actor);
    }

    private async void _removeRentAgreement()
    {
        db.CastingActors.Remove(CastingActors[SelectedIdx]);
        await db.SaveChangesAsync();
        CastingActors.RemoveAt(SelectedIdx);
    }

    private async void _updateRentAgreement()
    {
        CastingActors[SelectedIdx].Name = Name;
        CastingActors[SelectedIdx].Surname = Surname;
        CastingActors[SelectedIdx].Patronymic = Patronymic;

        db.CastingActors.Update(CastingActors[SelectedIdx]);
        await db.SaveChangesAsync();
    }
}