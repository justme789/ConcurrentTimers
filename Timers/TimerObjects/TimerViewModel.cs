using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Timers.ToDoList;

namespace Timers.TimerObjects
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private string _displayVal = "";
        private bool _isHovered = false;
        private DispatcherTimer _colorChanger;
        public Timer Timer { get; }
        public ObservableCollection<ToDoViewModel> ToDos { get; set; } = new ObservableCollection<ToDoViewModel>();
        public int RepeatedValue { get { return Timer.RepeatedValue; } }
        public bool IsRepeated
        {
            get => Timer.IsRepeated;
            set
            {
                Timer.IsRepeated = value;
                if (Timer.IsRepeated)
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
            get => Timer.CurrentValue;
            set => Timer.CurrentValue = value;
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
        public bool Cancelled
        {
            get => Timer.Cancelled; set
            {
                Timer.Cancelled = value;
                _colorChanger.Stop();
            }
        }
        public bool Created
        {
            get
            {
                return Timer.Created;
            }
            set
            {
                Timer.Created = value;
                if (Timer.Created)
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
                return Timer.Finished;
            }
            set
            {
                Timer.Finished = value;
                if (Timer.Finished)
                {
                    if (IsRepeated)
                    {
                        Timer.CurrentValue = Timer.OriginalValue;
                        DisplayVal = Timer.CurrentValue.ToString();
                        Timer.Finished = false;
                        Timer.RepeatedValue++;
                    }
                    else
                    {
                        CancelVisibility = Visibility.Visible;
                        DisplayVisibility = Visibility.Collapsed;
                        Timer.FinishTimer();
                        _colorChanger.Start();
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
            get { return Timer.IsPaused; }
            set
            {
                Timer.IsPaused = value;
                if (Timer.IsPaused)
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
        public Visibility SetupVisibility { get; set; } = Visibility.Visible;
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
        public delegate void RemoveTimerEventHandler(object sender, RemoveTimerEvent e);
        public event RemoveTimerEventHandler RemoveTimer;

        public delegate void ToDoAddedHandler(object sender, ToDoAdded e);
        public event ToDoAddedHandler ToDoAdded;

        public delegate void ToDoRemover(object sender, RemoveToDo e);
        public event ToDoRemover ToDoRemoved;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public TimerViewModel(Timer timer)
        {
            Timer = timer;

            BrushCreator();
            OriginalColor = Brush.Color;
            _colorChanger = new DispatcherTimer();
            _colorChanger.Interval = TimeSpan.FromMilliseconds(100);
            _colorChanger.Tick += ColorChanger_Tick;
            if (!timer.Created)
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
                Stages = new TimerCreator[3] { timerStage1, timerStage2, timerStage3 };
                CurrentStage = Stages[0];
            }
            else
            {
                InitializeTimer();
            }
        }
        public void CreateTimer()
        {
            long seconds = 0;
            for (int i = 0; i < Stages.Length; i++)
            {
                seconds += long.Parse(Stages[i].Value) * (long)Math.Pow(60, Stages.Length - 1 - i);
            }
            CurrentValue = seconds;
            Timer.OriginalValue = seconds;
            Created = true;
            InitializeTimer();
        }
        public void InitializeTimer()
        {
            TickerVisibility = Visibility.Visible;
            SetupVisibility = Visibility.Collapsed;
            SubVisibility = Visibility.Collapsed;
            CurrentValue = Timer.OriginalValue;
            DisplayVal = CurrentValue.ToString();
            IsRepeated = Timer.IsRepeated;
            OnPropertyChanged(nameof(TickerVisibility));
            OnPropertyChanged(nameof(SetupVisibility));
        }
        internal void Remove()
        {
            RemoveTimer?.Invoke(this, new RemoveTimerEvent { TimerToRemove = this });
        }
        private void ColorChanger_Tick(object? sender, EventArgs e)
        {
            BrushCreator();
            OnPropertyChanged("Brush");
            OnPropertyChanged("GlowColor");
        }
        public void BrushCreator()
        {
            Random rnd = new Random();
            int r = rnd.Next(160, 255);
            int g = rnd.Next(160, 255);
            int b = rnd.Next(160, 255);
            Brush = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            GlowColor = Brush.Color;
        }
        public void AddToDo(ToDo theToDoToAdd)
        {
            ToDoViewModel theViewModel = new ToDoViewModel(theToDoToAdd, Brush);
            theViewModel.ToDoRemoved += TheViewModel_ToDoRemoved;
            Timer.ToDos.Add(theToDoToAdd);
            ToDos.Add(theViewModel);
            ToDoAdded?.Invoke(this, new ToDoAdded { AddedToDo = theViewModel });
        }

        private void TheViewModel_ToDoRemoved(object sender, RemoveToDo e)
        {
            RemoveToDo(e.ToDoToRemove);
        }

        public void RemoveToDo(ToDoViewModel theToDoToRemove)
        {
            ToDos.Remove(theToDoToRemove);
            ToDoRemoved?.Invoke(this, new RemoveToDo { ToDoToRemove = theToDoToRemove });
        }
    }
}
