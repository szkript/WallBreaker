using System.Windows;

namespace Pong
{
    /// <summary>
    /// Interaction logic for Game_overWindow.xaml
    /// </summary>
    public partial class Game_overWindow : Window
    {
        private bool exitPressed { set; get; } = false;
        private bool restartPressed { set; get; } = false;

        public Game_overWindow(GameState title, int score)
        {
            InitializeComponent();
            GameOverScore.Content = score;
            Title.Content = title.ToString();
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            exitPressed = true;
            Close();
        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {

            restartPressed = true;
            Close();

        }
        public bool exitConfirmed()
        {
            return exitPressed;
        }

        public bool restartGame()
        {
            return restartPressed;
        }
    }
}
