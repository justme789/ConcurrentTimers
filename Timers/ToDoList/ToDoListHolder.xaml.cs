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
        private SolidColorBrush _timerBrush;
        private bool _closing;
        public SolidColorBrush TimerBrush
        {
            get => _timerBrush; set
            {
                _timerBrush = value;
                OnPropertyChanged(nameof(TimerBrush));
            }
        }
        public ToDoListHolder(TimerViewModel theParentTimer)
        {
            InitializeComponent();
            _timerBrush = theParentTimer.Brush;
            AllToDos.ItemsSource = ToDos;
            DataContext = this;
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!_closing)
            {
                this.Close();
                _closing = true;
            }
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
            ToDo toDo = new ToDo();
            ToDoViewModel todoViewModel = new ToDoViewModel(toDo);
            ToDos.Add(todoViewModel);
        }
    }
}
