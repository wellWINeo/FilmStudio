using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using Splat;

namespace FilmStudio.ViewModels;

public class CinemasViewModel : ViewModelBase
{
    // all cinemas from db
    public ObservableCollection<Cinema> Cinemas { get; set; }

    // index of selected cinema
    [Reactive] public int SelectedCinemaIndex { get; set; }

    // entity attributes
    [Reactive] public string Title { get; set; } = string.Empty;
    [Reactive] public string Address { get; set; } = string.Empty;

    // commands
    public ReactiveCommand<Unit, Unit> AddCinema { get; set; }
    public ReactiveCommand<Unit, Unit> UpdateCinema { get; set; }
    public ReactiveCommand<Unit, Unit> RemoveCinema { get; set; }

    // ctor
    public CinemasViewModel(IScreen screen) :
        base(screen)
    {
        // select cinemas 
        Cinemas = new(db.Cinemas);

        // init commands
        AddCinema = ReactiveCommand.Create(_addCinema, this.IsValid());
        UpdateCinema = ReactiveCommand.Create(_updateCinema, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedCinemaIndex, x => 0 <= x && x < Cinemas.Count),
            (x, y) => x && y
        ));
        RemoveCinema = ReactiveCommand.Create(_removeCinema, this.WhenAnyValue(
            x => x.SelectedCinemaIndex, x => 0 <= x && x < Cinemas.Count
        ));

        // validation
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

    // create & insert to database
    private void _addCinema()
    {
        var cinema = new Cinema()
        {
            Title = Title,
            Address = Address
        };
        db.Cinemas.Add(cinema);
        db.SaveChanges();
        Cinemas.Add(cinema);
    }

    // update cinema, currently selected in grid
    private void _updateCinema()
    {
        Cinemas[SelectedCinemaIndex].Title = Title;
        Cinemas[SelectedCinemaIndex].Address = Address;
        db.Cinemas.Update(Cinemas[SelectedCinemaIndex]);
        db.SaveChanges();
    }

    // remove selected cinema
    private void _removeCinema()
    {
        db.Cinemas.Remove(Cinemas[SelectedCinemaIndex]);
        db.SaveChanges();
        Cinemas.RemoveAt(SelectedCinemaIndex);
    }
}