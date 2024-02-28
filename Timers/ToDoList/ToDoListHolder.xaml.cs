using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private SolidColorBrush _timerBrush;
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
            DataContext = this;
        }
    }
}
