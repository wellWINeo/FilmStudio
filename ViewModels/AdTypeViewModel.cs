using System.Collections.ObjectModel;
using System.Reactive;
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
        UpdateAdType = ReactiveCommand.Create(_updateAdType, this.IsValid());
        RemoveAdType = ReactiveCommand.Create(_removeAdType);

    }

    private async void _adAdType()
    {
        var adType = new AdType() { Name = Name };
        await db.AdTypes.AddAsync(adType);
        await db.SaveChangesAsync();
        AdTypes.Add(adType);
    }

    private async void _removeAdType()
    {
        db.AdTypes.Remove(AdTypes[SelectedAdIndex]);
        await db.SaveChangesAsync();
        AdTypes.RemoveAt(SelectedAdIndex);
    }

    private async void _updateAdType()
    {
        if (SelectedAdIndex >= 0 && AdTypes[SelectedAdIndex].Name != Name)
        {
            AdTypes[SelectedAdIndex].Name = Name;
            db.AdTypes.Update(AdTypes[SelectedAdIndex]);
            await db.SaveChangesAsync();
        }
    }
}