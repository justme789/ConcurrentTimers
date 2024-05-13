using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Timers.ToDoList;

namespace Timers.Controls
{
    /// <summary>
    /// Interaction logic for CalendarTab.xaml
    /// </summary>
    public partial class CalendarTab : UserControl
    {
        public CalendarTab()
        {
            InitializeComponent();
            CalendarUI.ItemClicked += CalendarUI_ItemClicked;
        }

        private void CalendarUI_ItemClicked(object? sender, SelectedDateChanged e)
        {
            ObservableCollection<ToDoViewModel> todos = e.SelectedDate.ToDos;
            ToDoContainer.ItemsSource = todos;
        }

        private void AddToDOButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CalendarUI.SelectedDate != null)
            {
                ObservableCollection<ToDoViewModel> todos = CalendarUI.SelectedDate.ToDos;
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
                ToDoContainer.ItemsSource = todos;
                CalendarUI.SelectedDate.Refresh();
            }
        }
        private void TheViewModel_ToDoRemoved(object sender, RemoveToDo e)
        {
            ObservableCollection<ToDoViewModel> todos = CalendarUI.SelectedDate.ToDos;
            todos.Remove(e.ToDoToRemove);
            CalendarUI.SelectedDate.Refresh();
        }
    }
}
