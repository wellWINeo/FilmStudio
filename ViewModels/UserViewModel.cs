using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class UserViewModel : ViewModelBase
{
    // all users
    public ObservableCollection<User> Users { get; set; }

    [Reactive] public int SelectedIdx { get; set; }

    // user's attributes
    [Reactive] public string UserName { get; set; }
    [Reactive] public string Password { get; set; }

    // commands
    public ReactiveCommand<Unit, Unit> AddUser { get; }
    public ReactiveCommand<Unit, Unit> UpdateUser { get; }
    public ReactiveCommand<Unit, Unit> RemoveUser { get; }

    public UserViewModel(IScreen screen) : base(screen)
    {
        Users = new(db.Users);

        // validation
        this.ValidationRule(
            vm => vm.UserName,
            value => !string.IsNullOrWhiteSpace(value),
            "UserName can't be empty!"
        );

        this.ValidationRule(
            vm => vm.Password,
            value => !string.IsNullOrWhiteSpace(value),
            "Password can't be empty!"
        );


        // init commands
        AddUser = ReactiveCommand.Create(_addUser, this.IsValid());
        UpdateUser = ReactiveCommand.Create(_updateUser, Observable.CombineLatest(
            this.IsValid(), this.WhenAnyValue(x => x.SelectedIdx, x => 0 <= x && x < Users.Count),
            (x, y) => x && y
        // check is data valid & any user selected in grid for update
        ));
        RemoveUser = ReactiveCommand.Create(_removeUser, this.WhenAnyValue(
            x => x.SelectedIdx, x => 0 <= x && x < Users.Count
        // is index valid
        ));
    }

    private void _addUser()
    {
        var user = new User()
        {
            UserName = UserName,
            Password = Password
        };

        db.Users.Add(user);
        db.SaveChanges();
        Users.Add(user);
    }

    private void _removeUser()
    {
        db.Users.Remove(Users[SelectedIdx]);
        Users.RemoveAt(SelectedIdx);
        db.SaveChanges();
    }

    private void _updateUser()
    {
        Users[SelectedIdx].UserName = UserName;
        Users[SelectedIdx].Password = Password;
        db.Users.Update(Users[SelectedIdx]);
        db.SaveChanges();
    }
}