using System.Linq;
using System.Reactive;
using Avalonia.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FilmStudio.ViewModels;

public class LoginViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit> Login { get; }

    [Reactive] public string UserName { get; set; } = string.Empty;
    [Reactive] public string Password { get; set; } = string.Empty;
    [Reactive] public bool IsNotAuthed { get; set; } = true;

    public LoginViewModel(IScreen screen) :
        base(screen)
    {
        // init command
        Login = ReactiveCommand.Create(_login, this.WhenAnyValue(
            x => x.UserName, y => y.Password,
            (x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y)
        ));
    }

    // search user with entered username & password in db
    private void _login() =>
        IsNotAuthed = db.Users.Where(u => u.UserName == UserName &&
            u.Password == Password).FirstOrDefault() == null;
}