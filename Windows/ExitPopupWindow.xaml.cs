using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WallBreaker
{
    /// <summary>
    /// Interaction logic for ExitPopupWindow.xaml
    /// </summary>
    public partial class ExitPopupWindow : Window
    {
        private bool yesPressed { set; get; } = false;
        private bool restartPressed { set; get; } = false;

        public ExitPopupWindow()
        {
            InitializeComponent();
        }

        private void Exit_no_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Exit_yes_Click(object sender, RoutedEventArgs e)
        {
            yesPressed = true;
            Close();
        }
        public bool exitConfirmed()
        {
            return yesPressed;
        }

        private void Exit_restart_Click(object sender, RoutedEventArgs e)
        {
            restartPressed = true;
            Close();
        }

        public bool restartGame()
        {
            return restartPressed;
        }
    }
}
