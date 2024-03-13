using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Timers.TimerObjects;
using Timers.ToDoList;

namespace Timers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<TimersContainerViewModel> timers = new ObservableCollection<TimersContainerViewModel>();
        private ObservableCollection<ToDoViewModel> todos = new ObservableCollection<ToDoViewModel>();
        private string _theSaveLoadPath = Directory.GetCurrentDirectory() + @"\Project.json";

        public MainWindow()
        {
            InitializeComponent();
            TimersList.ItemsSource = timers;
            ToDoList.ItemsSource = todos;
            Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_theSaveLoadPath))
            {
                return;
            }
            string json = File.ReadAllText(_theSaveLoadPath);
            List<TimersContainer> containers = JsonConvert.DeserializeObject<List<TimersContainer>>(json);
            foreach (TimersContainer container in containers)
            {
                TimersContainerViewModel theContainerViewModel = new TimersContainerViewModel(container);
                theContainerViewModel.ToDoAdded += TheViewModel_ToDoAdded;
                theContainerViewModel.ToDoRemoved += TheViewModel_ToDoRemoved;
                foreach (Timer timer in container.GetTimers())
                {
                    theContainerViewModel.AddTimer(timer, true);
                }
                timers.Add(theContainerViewModel);
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            string theSavePath = Directory.GetCurrentDirectory() + @"\Project.json";
            List<TimersContainer> theContainersModel = new List<TimersContainer>();
            foreach (TimersContainerViewModel container in timers)
            {
                theContainersModel.Add(container.Container);
            }
            string json = JsonConvert.SerializeObject(theContainersModel);
            using (StreamWriter writer = new StreamWriter(theSavePath))
            {
                writer.Write(json);
            }
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
        private void TimerContainerAdder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimersContainer timersContainer = new TimersContainer();
            TimersContainerViewModel theViewModel = new TimersContainerViewModel(timersContainer);
            theViewModel.ToDoAdded += TheViewModel_ToDoAdded;
            theViewModel.ToDoRemoved += TheViewModel_ToDoRemoved;
            timers.Add(theViewModel);
        }

        private void TheViewModel_ToDoRemoved(object sender, RemoveToDo e)
        {
            todos.Remove(e.ToDoToRemove);
        }

        private void TheViewModel_ToDoAdded(object sender, ToDoAdded e)
        {
            todos.Add(e.AddedToDo);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                this.DragMove();
            }
            e.Handled = false;
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
        private void AddToDOButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToDo theTodoToAdd = new ToDo();
            ToDoViewModel theViewModel = new ToDoViewModel(theTodoToAdd, new SolidColorBrush(Colors.White));
            theViewModel.ToDoRemoved += TheViewModel_ToDoRemoved;
            if (todos.Any())
            {
                if (todos[^1].Created)
                {
                    todos.Add(theViewModel);
                }
            }
            else
            {
                todos.Add(theViewModel);
            }

        }
    }
}
