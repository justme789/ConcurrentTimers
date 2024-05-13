using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Timers.ToDoList;

namespace Timers.Controls
{
    /// <summary>
    /// Interaction logic for CalendarItem.xaml
    /// </summary>
    public class CalendarItemModel : INotifyPropertyChanged
    {
        public ObservableCollection<ToDoViewModel> ToDos { get; set; } = new ObservableCollection<ToDoViewModel>();
        public DateTime TheDate { get; set; }
        public Visibility IsVisible { get; set; } = Visibility.Collapsed;
        public Thickness TheMargin { get; set; } = new Thickness(0, 5, 10, 5);
        public Thickness TheBorderThickness { get; set; } = new Thickness(0);
        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected; set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    TheBorderThickness = new Thickness(2);
                }
                else TheBorderThickness = new Thickness(0);
                OnPropertyChanged(nameof(TheBorderThickness));
            }
        }
        public string MonthDay
        {
            get;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public CalendarItemModel(DateTime theDateTime)
        {
            TheDate = theDateTime;
            MonthDay = TheDate.ToString("dd");
            OnPropertyChanged(nameof(MonthDay));
        }
        public void Refresh()
        {
            if (ToDos.Any())
            {
                IsVisible = Visibility.Visible;
            }
            else IsVisible = Visibility.Collapsed;
            OnPropertyChanged(nameof(IsVisible));
        }
    }
    public partial class CalendarItem : UserControl
    {
        public CalendarItem()
        {
            InitializeComponent();
        }
    }
}
