using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Timers
{
    public class TimerSituation:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public long CurrVal { get; set; }
        private bool isHovered = false;
        public bool Hovered
        {
            get { return isHovered; }
            set
            {
                isHovered = value;
                if (isHovered)
                {
                    SubVisibility = Visibility.Visible;
                }
                else
                {
                    SubVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged("SubVisibility");
            }
        }
        public Visibility SubVisibility { get; set; }
        private string displayVal = "";
        public SolidColorBrush Brush { get; set; }
        public Color GlowColor { get; set; }
        public string DisplayVal {
            get { return displayVal; }
            set
            {
                long val = long.Parse(value);
                TimeSpan span = TimeSpan.FromSeconds(val);
                if(val < 60)
                {
                    displayVal = new DateTime( span.Ticks).ToString("ss");
                }
                else if(val>=60 && val < 3600)
                {
                    displayVal = new DateTime(span.Ticks).ToString("mm:ss");
                }
                else { displayVal = new DateTime(span.Ticks).ToString("HH:mm:ss"); }
                OnPropertyChanged("DisplayVal");
            }
        }

        

        public TimerSituation(long seconds)
        {
            CurrVal = seconds;
            Random rnd = new Random();
            int r = rnd.Next(130, 255);
            int g = rnd.Next(130, 255);
            int b = rnd.Next(130, 255);
            Brush = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            GlowColor = Brush.Color;
            DisplayVal = CurrVal.ToString();
        }
    }
}
