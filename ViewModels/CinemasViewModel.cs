using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class CinemasViewModel : ViewModelBase
{
    public ObservableCollection<Cinema> Cinemas { get; set; }

    [Reactive] public int SelectedCinemaIndex { get; set; }

    [Reactive] public string Title { get; set; } = string.Empty;
    [Reactive] public string Address { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> AddCinema { get; set; }
    public ReactiveCommand<Unit, Unit> UpdateCinema { get; set; }
    public ReactiveCommand<Unit, Unit> RemoveCinema { get; set; }


    public CinemasViewModel(ApplicationContext _db, IScreen screen) :
        base(_db, screen)
    {
        Cinemas = new(db.Cinemas);
        AddCinema = ReactiveCommand.Create(_addCinema, this.IsValid());
        UpdateCinema = ReactiveCommand.Create(_updateCinema, this.IsValid());
        RemoveCinema = ReactiveCommand.Create(_removeCinema, this.WhenAnyValue(
            x => x.SelectedCinemaIndex, x => 0 <= x && x < Cinemas.Count
        ));

        this.ValidationRule(
            vm => vm.Title,
            title => !string.IsNullOrWhiteSpace(title),
            "Title can't be empty!"
        );

        this.ValidationRule(
            vm => vm.Address,
            address => !string.IsNullOrWhiteSpace(address),
            "Address can't be empty!"
        );
    }

    private async void _addCinema()
    {
        var cinema = new Cinema()
        {
            Title = Title,
            Address = Address
        };
        await db.Cinemas.AddAsync(cinema);
        await db.SaveChangesAsync();
        Cinemas.Add(cinema);
    }

    private async void _updateCinema()
    {
        Cinemas[SelectedCinemaIndex].Title = Title;
        Cinemas[SelectedCinemaIndex].Address = Address;
        db.Cinemas.Update(Cinemas[SelectedCinemaIndex]);
        await db.SaveChangesAsync();
    }

    private async void _removeCinema()
    {
        db.Cinemas.Remove(Cinemas[SelectedCinemaIndex]);
        await db.SaveChangesAsync();
        Cinemas.RemoveAt(SelectedCinemaIndex);
    }
}