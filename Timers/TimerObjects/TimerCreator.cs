using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Timers.TimerObjects
{
    public class TimerCreator : INotifyPropertyChanged
    {
        public string Header { get; set; }
        public string Value { get; set; } = "00";
        public Visibility NextVisibility { get; set; }
        public Visibility ConfirmVisibility { get; set; }
        public Visibility PreviousVisibility { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
