using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Timers.TimerObjects;

namespace Timers.ToDoList
{
    /// <summary>
    /// Interaction logic for ToDoListHolder.xaml
    /// </summary>
    public partial class ToDoListHolder : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ToDoViewModel> ToDos { get; set; } = new ObservableCollection<ToDoViewModel>();
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private TimerViewModel _theParent;
        private SolidColorBrush _timerBrush;
        public SolidColorBrush TimerBrush
        {
            get => _theParent.Brush;
        }
        public ToDoListHolder(TimerViewModel theParentTimer)
        {
            InitializeComponent();
            _theParent = theParentTimer;
            AllToDos.ItemsSource = ToDos;
            DataContext = this;
            OnPropertyChanged(nameof(TimerBrush));
        }
        private void Window_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
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
            if (ToDos.Count > 0)
            {
                if (!ToDos[^1].Created)
                {
                    return;
                }
            }
            ToDo toDo = new ToDo(_theParent);
            ToDoViewModel todoViewModel = new ToDoViewModel(toDo);
            todoViewModel.RemoveToDo += TodoViewModel_RemoveToDo;
            ToDos.Add(todoViewModel);
        }

        private void TodoViewModel_RemoveToDo(object sender, RemoveToDo e)
        {
            ToDos.Remove(e.ToDoToRemove);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
