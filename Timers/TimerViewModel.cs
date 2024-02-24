using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Timers
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private string _displayVal = "";
        private bool _isHovered = false;
        private Timer _timer;
        public int RepeatedValue { get { return _timer.RepeatedValue; } }
        public bool IsRepeated
        {
            get => _timer.IsRepeated;
            set
            {
                _timer.IsRepeated = value;
                if (_timer.IsRepeated)
                {
                    RepeatedVisibility = Visibility.Visible;
                    Background = Brush;
                    Brush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    RepeatedVisibility = Visibility.Collapsed;
                    Brush = new SolidColorBrush(OriginalColor);
                    Background = new SolidColorBrush(Colors.Transparent);
                }
                OnPropertyChanged(nameof(RepeatedVisibility));
                OnPropertyChanged(nameof(Background));
                OnPropertyChanged(nameof(Brush));
            }
        }
        private TimerCreator _currentStage;
        public long CurrentValue
        {
            get => _timer.CurrentValue;
            set => _timer.CurrentValue = value;
        }
        public TimerCreator CurrentStage
        {
            get
            {
                return _currentStage;
            }
            set
            {
                _currentStage = value;
                OnPropertyChanged("CurrentStage");
            }
        }
        public TimerCreator[] Stages
        {
            get; set;
        }
        public bool Cancelled { get => _timer.Cancelled; set => _timer.Cancelled = value; }
        public bool Created
        {
            get
            {
                return _timer.Created;
            }
            set
            {
                _timer.Created = value;
                if (_timer.Created)
                {
                    TickerVisibility = Visibility.Visible;
                    OnPropertyChanged("TickerVisibility");
                }
            }
        }
        public bool Finished
        {
            get
            {
                return _timer.Finished;
            }
            set
            {
                _timer.Finished = value;
                if (_timer.Finished)
                {
                    if (IsRepeated)
                    {
                        _timer.CurrentValue = _timer.OriginalValue;
                        DisplayVal = _timer.CurrentValue.ToString();
                        _timer.Finished = false;
                        _timer.RepeatedValue++;
                    }
                    else
                    {
                        CancelVisibility = Visibility.Visible;
                        DisplayVisibility = Visibility.Collapsed;
                        _timer.FinishTimer();
                    }
                }
                else
                {
                    CancelVisibility = Visibility.Collapsed;
                    DisplayVisibility = Visibility.Collapsed;
                }
                OnPropertyChanged("CancelVisibility");
                OnPropertyChanged("DisplayVisibility");
                OnPropertyChanged("DisplayVisibility");
                OnPropertyChanged("RepeatedValue");
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
            get { return _timer.IsPaused; }
            set
            {
                _timer.IsPaused = value;
                if (_timer.IsPaused)
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
                    Brush = new SolidColorBrush(OriginalColor);
                    GlowColor = OriginalColor;
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
        public Visibility TickerVisibility { get; set; } = Visibility.Collapsed;
        public Visibility RepeatedVisibility { get; set; } = Visibility.Collapsed;
        public SolidColorBrush Brush { get; set; }
        public SolidColorBrush Background { get; set; } = new SolidColorBrush(Colors.Transparent);
        public Color GlowColor { get; set; }
        private Color OriginalColor { get; }
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
                else if (val >= 3600 && val < 24 * 3600)
                { _displayVal = new DateTime(span.Ticks).ToString("HH:mm:ss"); }
                else
                { _displayVal = new DateTime(span.Ticks).ToString("dd:HH:mm:ss"); }
                OnPropertyChanged("DisplayVal");
            }
        }

        public TimersContainer Container { get => _timer.Container; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public TimerViewModel(Timer timer)
        {
            TimerCreator timerStage1 = new TimerCreator();
            timerStage1.Header = "HH";
            timerStage1.ConfirmVisibility = Visibility.Collapsed;
            timerStage1.NextVisibility = Visibility.Visible;
            timerStage1.PreviousVisibility = Visibility.Collapsed;
            TimerCreator timerStage2 = new TimerCreator();
            timerStage2.ConfirmVisibility = Visibility.Collapsed;
            timerStage2.Header = "mm";
            timerStage2.NextVisibility = Visibility.Visible;
            timerStage2.PreviousVisibility = Visibility.Visible;
            TimerCreator timerStage3 = new TimerCreator();
            timerStage3.Header = "ss";
            timerStage3.ConfirmVisibility = Visibility.Visible;
            timerStage3.NextVisibility = Visibility.Collapsed;
            timerStage3.PreviousVisibility = Visibility.Visible;

            _timer = timer;
            DisplayVal = _timer.CurrentValue.ToString();
            Random rnd = new Random();
            int r = rnd.Next(130, 255);
            int g = rnd.Next(130, 255);
            int b = rnd.Next(130, 255);
            Brush = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            GlowColor = Brush.Color;
            OriginalColor = Brush.Color;
            Stages = new TimerCreator[3] { timerStage1, timerStage2, timerStage3 };
            CurrentStage = Stages[0];
        }
        public void CreateTimer()
        {
            long seconds = 0;
            for (int i = 0; i < Stages.Length; i++)
            {
                seconds += long.Parse(Stages[i].Value) * (long)Math.Pow(60, Stages.Length - 1 - i);
            }
            CurrentValue = seconds;
            _timer.OriginalValue = seconds;
            Created = true;
        }
    }
}
