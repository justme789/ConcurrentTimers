using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Timers
{
    public class Timer : INotifyPropertyChanged
    {
        private Thread _soundThread;
        private bool _finished = false;
        private string _displayVal = "";
        private bool _isHovered = false;
        private bool _isPaused = false;
        public long CurrVal { get; set; }
        public bool Cancelled { get; set; }
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
        public bool Hovered
        {
            get { return _isHovered; }
            set
            {
                _isHovered = value;
                if (_isHovered && !Finished)
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
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                if (_isPaused)
                {
                    PauseVisibility = Visibility.Collapsed;
                    PlayVisibility = Visibility.Visible;
                    Brush = new SolidColorBrush(Color.FromArgb(66, 122, 122, 122));
                    GlowColor = Brush.Color;
                }
                else
                {
                    PauseVisibility = Visibility.Visible;
                    PlayVisibility = Visibility.Collapsed;
                    Brush = new SolidColorBrush(_originalColor);
                    GlowColor = _originalColor;
                }
                OnPropertyChanged("PlayVisibility");
                OnPropertyChanged("PauseVisibility");
                OnPropertyChanged("Brush");
                OnPropertyChanged("GlowColor");
            }
        }
        public Visibility SubVisibility { get; set; }
        public Visibility CancelVisibility { get; set; } = Visibility.Collapsed;
        public Visibility DisplayVisibility { get; set; } = Visibility.Visible;
        public Visibility PauseVisibility { get; set; } = Visibility.Visible;
        public Visibility PlayVisibility { get; set; } = Visibility.Collapsed;
        public SolidColorBrush Brush { get; set; }
        public Color GlowColor { get; set; }
        private Color _originalColor;
        public string DisplayVal
        {
            get { return _displayVal; }
            set
            {
                long val = long.Parse(value);
                TimeSpan span = TimeSpan.FromSeconds(val);
                if (val < 60)
                {
                    _displayVal = new DateTime(span.Ticks).ToString("ss");
                }
                else if (val >= 60 && val < 3600)
                {
                    _displayVal = new DateTime(span.Ticks).ToString("mm:ss");
                }
                else { _displayVal = new DateTime(span.Ticks).ToString("HH:mm:ss"); }
                OnPropertyChanged("DisplayVal");
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
            _originalColor = Brush.Color;
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
