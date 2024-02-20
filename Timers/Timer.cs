using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Timers
{
    public class Timer : INotifyPropertyChanged
    {
        private Thread _soundThread;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public long CurrVal { get; set; }
        public bool Cancelled { get; set; }
        private bool _finished = false;
        public bool Finished
        {
            get
            {
                return _finished;
            }
            set
            {
                _finished = value; 
                if (_finished)
                {
                    CancelVisibility = Visibility.Visible;
                    DisplayVisibility = Visibility.Collapsed;
                }
                else
                {
                    CancelVisibility = Visibility.Collapsed;
                    DisplayVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged("CancelVisibility");
                OnPropertyChanged("DisplayVisibility");
                FinishTimer();
            }
        }
        private bool isHovered = false;
        public bool Hovered
        {
            get { return isHovered; }
            set
            {
                isHovered = value;
                if (isHovered && !Finished)
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
        public Visibility CancelVisibility { get; set; } = Visibility.Collapsed;
        public Visibility DisplayVisibility { get; set; } = Visibility.Visible;
        private string displayVal = "";
        public SolidColorBrush Brush { get; set; }
        public Color GlowColor { get; set; }
        public string DisplayVal
        {
            get { return displayVal; }
            set
            {
                long val = long.Parse(value);
                TimeSpan span = TimeSpan.FromSeconds(val);
                if (val < 60)
                {
                    displayVal = new DateTime(span.Ticks).ToString("ss");
                }
                else if (val >= 60 && val < 3600)
                {
                    displayVal = new DateTime(span.Ticks).ToString("mm:ss");
                }
                else { displayVal = new DateTime(span.Ticks).ToString("HH:mm:ss"); }
                OnPropertyChanged("DisplayVal");
            }
        }

        public Timer(long seconds)
        {
            CurrVal = seconds;
            Random rnd = new Random();
            int r = rnd.Next(130, 255);
            int g = rnd.Next(130, 255);
            int b = rnd.Next(130, 255);
            Brush = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            GlowColor = Brush.Color;
            DisplayVal = CurrVal.ToString();
            _soundThread = new Thread(() =>
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\nobod\source\repos\Timers\Timers\soft.wav");
                player.Play();
                while (!Cancelled) { }
                player.Stop();
            });
        }
        public void FinishTimer()
        {
            _soundThread.Start();
        }
    }
}
