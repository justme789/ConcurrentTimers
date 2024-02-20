using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Timers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TimersContainer> timers = new ObservableCollection<TimersContainer>();
        public MainWindow()
        {
            InitializeComponent();
            TimersList.ItemsSource = timers;
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                Random rnd = new Random();
                timers[rnd.Next(timers.Count)].Timers.Add(new Timer(rnd.Next(10)));
            }
            if (e.Key == Key.C)
            {
                Random rnd = new Random();
                timers.Add(new TimersContainer());
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((TimersContainer)((Border)sender).DataContext).Timers.Add(new Timer(10));
        }

        private void Border_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            timers.Add(new TimersContainer());
        }
    }
}
