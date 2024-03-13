using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Timers.TimerObjects
{
    /// <summary>
    /// This class handles the creation stages of a timer
    /// </summary>
    public class TimerCreator : INotifyPropertyChanged
    {
        /// <summary>
        /// The top header of the timer creator, HH, MM, SS
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// The current value for the respective time unit
        /// </summary>
        public string Value { get; set; } = "00";
        /// <summary>
        /// The visibilities of specific buttons at different stages
        /// </summary>
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
