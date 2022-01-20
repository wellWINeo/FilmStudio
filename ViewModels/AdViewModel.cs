using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Linq;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using Splat;

namespace FilmStudio.ViewModels;

public class AdViewModel : ViewModelBase
{
    // sources
    public ObservableCollection<Ad> Ads { get; set; }
    public ObservableCollection<Movie> Movies { get; set; }
    public ObservableCollection<AdType> AdTypes { get; set; }

    // indexes
    [Reactive] public int SelectedAdIdx { get; set; }
    [Reactive] public int SelectedMovieIdx { get; set; }
    [Reactive] public int SelectedAdTypeIdx { get; set; }

    // attributes
    [Reactive] public string Source { get; set; } = string.Empty;
    [Reactive] public decimal Amount { get; set; } = 0.0M;
    [Reactive] public string TargetAudience { get; set; } = string.Empty;

    // comands
    public ReactiveCommand<Unit, Unit> AddAd { get; set; }
    public ReactiveCommand<Unit, Unit> RemoveAd { get; set; }
    public ReactiveCommand<Unit, Unit> UpdateAd { get; set; }


    private Movie _castMovieFromId => Movies.ElementAt(SelectedMovieIdx);
    private AdType _castAdTypeFromId
        => AdTypes.ElementAt(SelectedAdTypeIdx);

    public AdViewModel(IScreen screen) : base(screen)
    {
        // select ads with eager loading
        Ads = new(db.Ads
            .Include(e => e.Movie)
            .Include(e => e.AdType)
            .ToList());
        // all movies & ad types
        Movies = new(db.Movies);
        AdTypes = new(db.AdTypes);

        // validation
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

        // init commands
        AddAd = ReactiveCommand.Create(_addAd, this.IsValid());
        UpdateAd = ReactiveCommand.Create(_updateAd, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedAdIdx, x => 0 <= x && x < Ads.Count),
            (x, y) => x && y
        ));
        RemoveAd = ReactiveCommand.Create(_removeAd, this.WhenAnyValue(
            x => x.SelectedAdIdx, x => 0 <= x && x < Ads.Count
        ));
    }

    private void _addAd()
    {
        var ad = new Ad()
        {
            Source = Source,
            Amount = Amount,
            TargetAudience = TargetAudience,
            Movie = _castMovieFromId,
            AdType = _castAdTypeFromId
        };

        db.Ads.Add(ad);
        db.SaveChanges();
        Ads.Add(ad);
    }

    private void _removeAd()
    {
        db.Ads.Remove(Ads[SelectedAdIdx]);
        db.SaveChanges();
        Ads.RemoveAt(SelectedAdIdx);
    }

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