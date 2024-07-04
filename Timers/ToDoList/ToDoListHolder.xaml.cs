using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Timers.Messengers;
using Timers.TimerObjects;

namespace Timers.ToDoList
{
    /// <summary>
    /// Interaction logic for ToDoListHolder.xaml
    /// </summary>
    public partial class ToDoListHolder : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private TimerViewModel _theParent;
        public SolidColorBrush TimerBrush
        {
            get => _theParent.Brush;
        }
        public ToDoListHolder(TimerViewModel theParentTimer)
        {
            InitializeComponent();
            _theParent = theParentTimer;
            AllToDos.ItemsSource = theParentTimer.ToDos;
            AvailableToDos.ItemsSource = ToDoMessenger.Instance.TodaysTodos;
            DataContext = this;
            OnPropertyChanged(nameof(TimerBrush));
        }
        private void Window_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Make sure user is dragging a draggable object
            Type sourceType = e.OriginalSource.GetType();
            PropertyInfo tagProperty = sourceType.GetProperty("Tag");
            if (tagProperty != null)
            {
                object tag = tagProperty.GetValue(e.OriginalSource);
                if (tag != null && tag.Equals("Back"))
                {
                    if (e.ChangedButton == MouseButton.Left)
                    {
                        this.DragMove();
                    }
                    e.Handled = false;
                }
            }
        }
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Handles adding a new todo
            ToDo toDo = new ToDo();
            _theParent.AddToDo(toDo);
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private void AllToDos_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ToDoItem todoitem = FindAncestor<ToDoItem>((DependencyObject)e.OriginalSource);

            if (todoitem != null && ((ToDoViewModel)todoitem.DataContext).Created)
            {
                DragDrop.DoDragDrop(todoitem, ((ToDoViewModel)todoitem.DataContext), DragDropEffects.Move);
            }
        }

        private void AvailableToDos_Drop(object sender, DragEventArgs e)
        {

        }

        private void AllToDos_Drop(object sender, DragEventArgs e)
        {
            _theParent.AddToDo((ToDoViewModel)e.Data.GetData(typeof(ToDoViewModel)));
        }
    }
}
