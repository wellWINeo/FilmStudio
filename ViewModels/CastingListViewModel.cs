using System;
using System.Collections.ObjectModel;
using FilmStudio.Models;
using ReactiveUI;

namespace FilmStudio.ViewModels;

public class CastingListViewModel : ViewModelBase
{
    private ApplicationContext db;

    public ObservableCollection<CastingList> CastingLists { get; set; }

    public CastingListViewModel(ApplicationContext _db)
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
