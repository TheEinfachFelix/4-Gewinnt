using System.Windows;

namespace _4_Gewinnt_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var controller = new GameController(BoardGrid, 7, 6); // Standardgröße 7x6
        }
    }
}
