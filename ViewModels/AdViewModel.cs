using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Linq;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FilmStudio.ViewModels;

public class AdViewModel : ViewModelBase
{
    public ObservableCollection<Ad> Ads { get; set; }
    public IEnumerable<Movie> Movies { get; set; }
    public IEnumerable<AdType> AdTypes { get; set; }

    [Reactive] public int SelectedAdIdx { get; set; }
    [Reactive] public int SelectedMovieIdx { get; set; }
    [Reactive] public int SelectedAdTypeIdx { get; set; }

    [Reactive] public string Source { get; set; } = string.Empty;
    [Reactive] public decimal Amount { get; set; } = 0.0M;
    [Reactive] public string TargetAudience { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> AddAd { get; set; }
    public ReactiveCommand<Unit, Unit> RemoveAd { get; set; }
    public ReactiveCommand<Unit, Unit> UpdateAd { get; set; }


    private Movie _castMovieFromId => Movies.ElementAt(SelectedMovieIdx);
    private AdType _castAdTypeFromId
        => AdTypes.ElementAt(SelectedAdTypeIdx);

    public AdViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        Ads = new(db.Ads
            .Include(e => e.Movie)
            .Include(e => e.AdType)
            .ToList());
        Movies = db.Movies.AsEnumerable();
        AdTypes = db.AdTypes.AsEnumerable();

        this.ValidationRule(
            vm => vm.Source,
            value => !string.IsNullOrEmpty(value),
            "Source can't be empty!"
        );

        this.ValidationRule(
            vm => vm.Amount,
            value => value >= 0,
            "Amount can't be negative!"
        );

        this.ValidationRule(
            vm => vm.TargetAudience,
            value => !string.IsNullOrEmpty(value),
            "Target audience can't be empty!"
        );

        this.ValidationRule(
            vm => vm.SelectedMovieIdx,
            value => value >= 0 && value < Movies.Count(),
            "Choose movie!"
        );

        this.ValidationRule(
            vm => vm.SelectedAdTypeIdx,
            value => value >= 0 && value < AdTypes.Count(),
            "Choose ad type!"
        );

        AddAd = ReactiveCommand.Create(_addAd, this.IsValid());
        UpdateAd = ReactiveCommand.Create(_updateAd, this.IsValid());
        RemoveAd = ReactiveCommand.Create(_removeAd, this.WhenAnyValue(
            x => x.SelectedAdIdx, x => 0 <= x && x < Ads.Count
        ));
    }

    private async void _addAd()
    {
        var ad = new Ad()
        {
            Source = Source,
            Amount = Amount,
            TargetAudience = TargetAudience,
            Movie = _castMovieFromId,
            AdType = _castAdTypeFromId
        };

        await db.Ads.AddAsync(ad);
        await db.SaveChangesAsync();
        Ads.Add(ad);
    }

    private async void _removeAd()
    {
        db.Ads.Remove(Ads[SelectedAdIdx]);
        await db.SaveChangesAsync();
        Ads.RemoveAt(SelectedAdIdx);
    }

    // FIXME: multiple updates cause exception
    private void _updateAd()
    {
        Ads[SelectedAdIdx].Source = Source;
        Ads[SelectedAdIdx].Amount = Amount;
        Ads[SelectedAdIdx].TargetAudience = TargetAudience;
        Ads[SelectedAdIdx].Movie = Movies.ElementAt(SelectedMovieIdx);
        Ads[SelectedAdIdx].AdType = AdTypes.ElementAt(SelectedAdTypeIdx);


        db.Ads.Update(Ads[SelectedAdIdx]);
        db.SaveChanges();
    }

}