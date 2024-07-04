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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Timers.Controls
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        private MainWindow _theParent;
        public TitleBar()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (_theParent.WindowState == WindowState.Maximized)
                {
                    _theParent.WindowState = WindowState.Normal;
                }
                _theParent.DragMove();
            }
            e.Handled = false;
        }
        public void test() { }
        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Border theSender = sender as Border;
            theSender.Background = new SolidColorBrush(Color.FromArgb(44, 255, 0, 0));
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Border theSender = sender as Border;
            theSender.Background = new SolidColorBrush(Colors.Transparent);

        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border theSender = sender as Border;
            theSender.Background = new SolidColorBrush(Color.FromArgb(100, 19, 173, 252));
        }

        private void Minimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _theParent.WindowState = WindowState.Minimized;
        }

        private void Maximize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _theParent.WindowState = _theParent.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _theParent.Close();
        }

        internal void SetParentWindow(MainWindow mainWindow)
        {
            _theParent = mainWindow;
        }
    }
}
