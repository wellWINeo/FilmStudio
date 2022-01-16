using System;
using System.Collections.ObjectModel;
using FilmStudio.Models;
using ReactiveUI;

namespace FilmStudio.ViewModels;

public class CastingListViewModel : ViewModelBase
{
    public ObservableCollection<CastingList> CastingLists { get; set; }

    public CastingListViewModel(ApplicationContext _db, IScreen screen) :
        base(_db, screen)
    {
        db = _db;
        CastingLists = new ObservableCollection<CastingList>()
        {
            new CastingList{},
            new CastingList{},
            new CastingList{}
        };
    }
}
