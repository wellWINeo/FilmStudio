using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using ReactiveUI.Validation.Helpers;
using Splat;

namespace FilmStudio.ViewModels;
public class ViewModelBase : ReactiveValidationObject, IRoutableViewModel
{
    protected ApplicationContext db;
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public IScreen HostScreen { get; }

    public ViewModelBase(IScreen screen)
    {
        // resolve context using locator
        db = Locator.Current.GetService<ApplicationContext>() ??
            throw new System.Exception("Can't locate 'ApplicaitionContex'");
        HostScreen = screen;
    }
}
