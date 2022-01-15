using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;

namespace FilmStudio.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EmployeeViewModel employeeViewModel { get; set; }
        public CastingListViewModel castingListViewModel { get; set; }

        private readonly Interaction<Unit, bool> login;
        public Interaction<Unit, bool> Login => this.login;
        public string Greeting => "Welcome to Avalonia!";

        public MainWindowViewModel(EmployeeViewModel _employeeViewModel,
                                CastingListViewModel _castingListViewModel)
        {
            login = new();
            var isLogin = this.login.Handle(new Unit());
            employeeViewModel = _employeeViewModel;
            castingListViewModel = _castingListViewModel;
        }
    }

}
