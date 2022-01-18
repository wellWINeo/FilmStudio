using System.Collections.ObjectModel;
using System.Reactive;
using FilmStudio.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace FilmStudio.ViewModels;

public class UserViewModel : ViewModelBase
{
    public ObservableCollection<User> Users { get; set; }

    [Reactive] public int SelectedIdx { get; set; }

    [Reactive] public string UserName { get; set; }
    [Reactive] public string Password { get; set; }

    public ReactiveCommand<Unit, Unit> AddUser { get; }
    public ReactiveCommand<Unit, Unit> UpdateUser { get; }
    public ReactiveCommand<Unit, Unit> RemoveUser { get; }

    public UserViewModel(ApplicationContext _db, IScreen screen) : base(_db, screen)
    {
        Users = new(db.Users);

        // TODO: validation

        AddUser = ReactiveCommand.Create(_addUser, this.IsValid());
        UpdateUser = ReactiveCommand.Create(_updateUser, this.IsValid());
        RemoveUser = ReactiveCommand.Create(_removeUser);
    }

    private async void _addUser()
    {
        var user = new User()
        {
            UserName = UserName,
            Password = Password
        };

        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
        Users.Add(user);
    }

    private async void _removeUser()
    {
        db.Users.Remove(Users[SelectedIdx]);
        await db.SaveChangesAsync();
        Users.RemoveAt(SelectedIdx);
    }

    private async void _updateUser()
    {
        Users[SelectedIdx].UserName = UserName;
        Users[SelectedIdx].Password = Password;
        db.Users.Update(Users[SelectedIdx]);
        await db.SaveChangesAsync();
    }
}