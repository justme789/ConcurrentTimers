using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using Timers.TimerObjects;
using Timers.ToDoList;

namespace Timers.Controls
{
    /// <summary>
    /// Interaction logic for TimersList.xaml
    /// </summary>
    public partial class TimersList : UserControl
    {
        private ObservableCollection<TimersContainerViewModel> timers = new ObservableCollection<TimersContainerViewModel>();
        public TimersList()
        {
            InitializeComponent();
            ListOfTimers.ItemsSource = timers;
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
            //todos.Remove(e.ToDoToRemove);
        }

        private void TheViewModel_ToDoAdded(object sender, ToDoAdded e)
        {
            //todos.Add(e.AddedToDo);
        }
    }
}
