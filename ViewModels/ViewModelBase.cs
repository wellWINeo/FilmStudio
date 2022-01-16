using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using ReactiveUI.Validation.Helpers;

namespace FilmStudio.ViewModels;
public class ViewModelBase : ReactiveValidationObject, IRoutableViewModel
{
    protected ApplicationContext db;
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public IScreen HostScreen { get; }

    public ViewModelBase(ApplicationContext _db, IScreen screen)
    {
        db = _db;
        HostScreen = screen;
    }
}
