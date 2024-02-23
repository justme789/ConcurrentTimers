using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                timers[rnd.Next(timers.Count)].Timers.Add(new Timer());
            }
            if (e.Key == Key.C)
            {
                Random rnd = new Random();
                timers.Add(new TimersContainer());
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((TimersContainer)((Border)sender).DataContext).Timers.Add(new Timer());
        }

        private void Border_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            timers.Add(new TimersContainer());
        }
    }
}
