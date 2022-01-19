using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class AdTypeViewModel : ViewModelBase
{
    public ObservableCollection<AdType> AdTypes { get; set; }

    [Reactive] public int SelectedAdIndex { get; set; } = 0;
    [Reactive] public string Name { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> AddAdType { get; }
    public ReactiveCommand<Unit, Unit> RemoveAdType { get; }
    public ReactiveCommand<Unit, Unit> UpdateAdType { get; }


    public AdTypeViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        AdTypes = new(db.AdTypes);

        this.ValidationRule(
            vm => vm.Name,
            name => !string.IsNullOrWhiteSpace(name),
            "Ad type name can't be empty!"
        );

        AddAdType = ReactiveCommand.Create(_adAdType, this.IsValid());
        UpdateAdType = ReactiveCommand.Create(_updateAdType, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedAdIndex, x => 0 <= x && x < AdTypes.Count),
            (x, y) => x && y
        ));
        RemoveAdType = ReactiveCommand.Create(_removeAdType, this.WhenAnyValue(
            x => x.SelectedAdIndex, x => 0 <= x && x < AdTypes.Count
        ));

    }

    private void _adAdType()
    {
        var adType = new AdType() { Name = Name };
        db.AdTypes.Add(adType);
        db.SaveChanges();
        AdTypes.Add(adType);
    }

    private void _removeAdType()
    {
        db.AdTypes.Remove(AdTypes[SelectedAdIndex]);
        db.SaveChanges();
        AdTypes.RemoveAt(SelectedAdIndex);
    }

    private void _updateAdType()
    {
        if (SelectedAdIndex >= 0 && AdTypes[SelectedAdIndex].Name != Name)
        {
            AdTypes[SelectedAdIndex].Name = Name;
            db.AdTypes.Update(AdTypes[SelectedAdIndex]);
            db.SaveChanges();
        }
    }
}