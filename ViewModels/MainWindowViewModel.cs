using System;
using System.Collections.Generic;
using System.Text;

namespace FilmStudio.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EmployeeViewModel employeeViewModel { get; set; }
        public CastingListViewModel castingListViewModel { get; set; }
        public string Greeting => "Welcome to Avalonia!";

        public MainWindowViewModel(EmployeeViewModel _employeeViewModel,
                                CastingListViewModel _castingListViewModel)
        {
            employeeViewModel = _employeeViewModel;
            castingListViewModel = _castingListViewModel;
        }
    }
}
