using System.Linq;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FilmStudio.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private ApplicationContext db;

    public ReactiveCommand<Unit, Unit> Login { get; }

    [Reactive] public string UserName { get; set; } = string.Empty;
    [Reactive] public string Password { get; set; } = string.Empty;
    [Reactive] public bool IsNotAuthed { get; set; } = true;

    public LoginViewModel(ApplicationContext _db)
    {
        db = _db;
        Login = ReactiveCommand.Create(_login);
    }

    private void _login()
    {
        // if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
        //     IsNotAuthed = false;
        IsNotAuthed = db.Users.Where(u => u.UserName == UserName &&
            u.Password == Password).FirstOrDefault() == null;
    }
}