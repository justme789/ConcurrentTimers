using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Timers.TimerObjects;
using Timers.ToDoList;

namespace Timers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ToDoViewModel> todos = new ObservableCollection<ToDoViewModel>();
        private string _theSaveLoadPath = Directory.GetCurrentDirectory() + @"\Project.json";

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;
            TheTitleBar.SetParentWindow(this);
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
                //timers.Add(theContainerViewModel);
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            string theSavePath = Directory.GetCurrentDirectory() + @"\Project.json";
            List<TimersContainer> theContainersModel = new List<TimersContainer>();
            /*foreach (TimersContainerViewModel container in timers)
            {
                theContainersModel.Add(container.Container);
            }
            string json = JsonConvert.SerializeObject(theContainersModel);
            using (StreamWriter writer = new StreamWriter(theSavePath))
            {
                writer.Write(json);
            }*/
        }
        private void TheViewModel_ToDoRemoved(object sender, RemoveToDo e)
        {
            todos.Remove(e.ToDoToRemove);
        }

        private void TheViewModel_ToDoAdded(object sender, ToDoAdded e)
        {
            todos.Add(e.AddedToDo);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox theSender = sender as ListBox;
            if (theSender.SelectedIndex == 0)
            {
                TimersListContainer.Visibility = Visibility.Visible;
                CalendarContainer.Visibility = Visibility.Collapsed;
            }
            else
            {
                TimersListContainer.Visibility = Visibility.Collapsed;
                CalendarContainer.Visibility = Visibility.Visible;
            }
        }
    }
}
