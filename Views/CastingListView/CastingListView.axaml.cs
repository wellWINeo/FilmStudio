using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FilmStudio.Views
{
    public partial class CastingListView : UserControl
    {
        public CastingListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}