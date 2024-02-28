using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Timers.TimerObjects;

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
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Timer File|*.TIMER";
                saveDialog.Title = "Save Timer";
                saveDialog.ShowDialog();
                if (saveDialog.FileName != "")
                {
                    string json = JsonConvert.SerializeObject(actual);
                    using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                    {
                        writer.Write(json);
                    }
                }
            }
            if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "Timer File|*.TIMER";
                openDialog.Title = "Save Timer";
                openDialog.ShowDialog();
                if (openDialog.FileName != "")
                {
                    actual.Clear();
                    timers.Clear();
                    string json = File.ReadAllText(openDialog.FileName);
                    List<TimersContainer> containers = JsonConvert.DeserializeObject<List<TimersContainer>>(json);
                    foreach (TimersContainer container in containers)
                    {
                        actual.Add(container);
                        timers.Add(new TimersContainerViewModel(container));
                    }
                }
            }
        }

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
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
