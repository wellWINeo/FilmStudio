using System;
using System.Collections.ObjectModel;
using FilmStudio.Models;
using ReactiveUI;

namespace FilmStudio.ViewModels;

public class CastingListViewModel : ViewModelBase
{
    private ApplicationContext db;

    public ObservableCollection<CastingList> castingLists { get; set; }

    public CastingListViewModel(ApplicationContext _db)
    {
        db = _db;
        castingLists = new ObservableCollection<CastingList>()
        {
            new CastingList{},
            new CastingList{},
            new CastingList{}
        };
    }
}
