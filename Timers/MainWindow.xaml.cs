using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        ObservableCollection<TimersContainerViewModel> timers = new ObservableCollection<TimersContainerViewModel>();
        List<TimersContainer> actual = new List<TimersContainer>();
        Dictionary<TimerViewModel, TimersContainerViewModel> timerToContainer = new Dictionary<TimerViewModel, TimersContainerViewModel>();
        public MainWindow()
        {
            InitializeComponent();
            TimersList.ItemsSource = timers;
        }
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimersContainerViewModel currentDataContext = (TimersContainerViewModel)((Border)sender).DataContext;
            if (currentDataContext.Timers.Count > 0)
            {
                if (currentDataContext.Timers[^1].Finished || !currentDataContext.Timers[^1].Created)
                {
                    return;
                }
            }
            Timer timer = new Timer();
            currentDataContext.AddTimer(timer);
        }
        private void Border_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            TimersContainer timersContainer = new TimersContainer();
            TimersContainerViewModel theViewModel = new TimersContainerViewModel(timersContainer);
            timers.Add(theViewModel);
            actual.Add(timersContainer);
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
            e.Handled = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
            {
                string json = JsonConvert.SerializeObject(actual);
                using (StreamWriter writer = new StreamWriter(@"C:\Users\nobod\OneDrive\Desktop\tttt.json"))
                {
                    writer.Write(json);
                }
            }
            if (e.Key == Key.D)
            {
                string json = File.ReadAllText(@"C:\Users\nobod\OneDrive\Desktop\tttt.json");
                List<TimersContainer> containers = JsonConvert.DeserializeObject<List<TimersContainer>>(json);
                foreach (TimersContainer container in containers)
                {
                    actual.Add(container);
                    timers.Add(new TimersContainerViewModel(container));
                }
            }
        }
    }
}
