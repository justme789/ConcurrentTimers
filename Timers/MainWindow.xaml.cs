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
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimersContainer currentDataContext = (TimersContainer)((Border)sender).DataContext;
            if (currentDataContext.Timers.Count > 0)
            {
                if (currentDataContext.Timers[^1].Finished || !currentDataContext.Timers[^1].Created)
                {
                    return;
                }
            }
            Timer timer = new Timer { Container = currentDataContext };
            currentDataContext.Timers.Add(new TimerViewModel(timer));
            e.Handled = true;
        }

        private void Border_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            timers.Add(new TimersContainer());
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
            e.Handled = false;
        }
    }
}
