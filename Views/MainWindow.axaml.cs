using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FilmStudio.ViewModels;
using PropertyChanged;
using ReactiveUI;

namespace FilmStudio.Views
{
    [DoNotNotify]
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(disposable =>
            {
                disposable(this
                        .ViewModel!
                        .Login
                        .RegisterHandler(
                            async interaction =>
                            {
                                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                                    .GetMessageBoxStandardWindow("title", "Test");
                                await messageBoxStandardWindow?.Show();
                            }
                        ));
            });
#if DEBUG
            this.AttachDevTools();
#endif

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


    }
}