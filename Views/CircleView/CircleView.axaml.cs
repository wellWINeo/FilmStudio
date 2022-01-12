using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FilmStudio.Views
{
    public partial class CircleView : UserControl
    {
        public CircleView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}