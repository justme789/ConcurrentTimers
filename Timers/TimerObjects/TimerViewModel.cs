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
    /// <summary>
    /// This is the view model of a timer
    /// </summary>
    public class TimerViewModel : INotifyPropertyChanged
    {
        private string _displayVal = "";
        private bool _isHovered = false;
        private DispatcherTimer _colorChanger;
        private TimerCreator _currentStage;
        /// <summary>
        /// The original brush value when the timer is created
        /// This value is used whenever the timer color changes for example
        /// when it is repeated or paused
        /// </summary>
        private readonly Color _originalColor;
        /// <summary>
        /// The timer model
        /// </summary>
        public Timer Timer { get; }
        /// <summary>
        /// The stored todos
        /// </summary>
        public ObservableCollection<ToDoViewModel> ToDos { get; set; } = new ObservableCollection<ToDoViewModel>();
        /// <summary>
        /// How many times this timer has been repeated
        /// </summary>
        public int RepeatedValue { get { return Timer.RepeatedValue; } }
        /// <summary>
        /// Wether this timer is repeated
        /// </summary>
        public bool IsRepeated
        {
            get => Timer.IsRepeated;
            set
            {
                Timer.IsRepeated = value;
                if (Timer.IsRepeated)
                {
                    // Flip the color scheme of the timer
                    RepeatedVisibility = Visibility.Visible;
                    Background = Brush;
                    Brush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    // Bring the color scheme back
                    RepeatedVisibility = Visibility.Collapsed;
                    Brush = new SolidColorBrush(_originalColor);
                    Background = new SolidColorBrush(Colors.Transparent);
                }
                OnPropertyChanged(nameof(RepeatedVisibility));
                OnPropertyChanged(nameof(Background));
                OnPropertyChanged(nameof(Brush));
            }
        }
        /// <summary>
        /// The time in seconds of the timer
        /// </summary>
        public long CurrentValue
        {
            get => Timer.CurrentValue;
            set => Timer.CurrentValue = value;
        }
        /// <summary>
        /// The current timer creation stage
        /// </summary>
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
        /// <summary>
        /// The stages the timer goes through to get created ie ss. mm. hh
        /// </summary>
        public TimerCreator[] Stages { get; set; }
        /// <summary>
        /// Wether the timer is cancelled
        /// </summary>
        public bool Cancelled
        {
            get => Timer.Cancelled; set
            {
                Timer.Cancelled = value;
                _colorChanger.Stop();
            }
        }
        /// <summary>
        /// Flag that timer passed all the creation stages
        /// </summary>
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
                    // Show the actual timer value when done
                    TickerVisibility = Visibility.Visible;
                    OnPropertyChanged("TickerVisibility");
                }
            }
        }
        /// <summary>
        /// Flag that represented if the timer reached 0
        /// </summary>
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
                        // Just repeat if the timer is repeated
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
        /// <summary>
        /// Show extra info on hover
        /// </summary>
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
        /// <summary>
        /// Handles what happens when a timer is paused
        /// </summary>
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
                    Brush = new SolidColorBrush(_originalColor);
                    GlowColor = _originalColor;
                }
                OnPropertyChanged("PlayVisibility");
                OnPropertyChanged("PauseVisibility");
                OnPropertyChanged("Brush");
                OnPropertyChanged("GlowColor");
            }
        }
        /// <summary>
        /// All the visibility stuff
        /// </summary>
        public Visibility SubVisibility { get; set; }
        public Visibility CancelVisibility { get; set; } = Visibility.Collapsed;
        public Visibility DisplayVisibility { get; set; } = Visibility.Visible;
        public Visibility PauseVisibility { get; set; } = Visibility.Visible;
        public Visibility PlayVisibility { get; set; } = Visibility.Collapsed;
        public Visibility TickerVisibility { get; set; } = Visibility.Collapsed;
        public Visibility SetupVisibility { get; set; } = Visibility.Visible;
        public Visibility RepeatedVisibility { get; set; } = Visibility.Collapsed;
        /// <summary>
        /// All the colors
        /// </summary>
        public SolidColorBrush Brush { get; set; }
        /// <summary>
        /// Used when the timer is repeated
        /// </summary>
        public SolidColorBrush Background { get; set; } = new SolidColorBrush(Colors.Transparent);
        public Color GlowColor { get; set; }
        /// <summary>
        /// The actual display that a user sees
        /// COnversion from seconds to days, hours, minutes, seconds
        /// </summary>
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
        /// <summary>
        /// The event that this timer has been removed. Gets subscribed to in the container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RemoveTimerEventHandler(object sender, RemoveTimerEvent e);
        public event RemoveTimerEventHandler RemoveTimer;
        /// <summary>
        /// Todo evemts that propogate all the way up to the main application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ToDoAddedHandler(object sender, ToDoAdded e);
        public event ToDoAddedHandler ToDoAdded;
        public delegate void ToDoRemover(object sender, RemoveToDo e);
        public event ToDoRemover ToDoRemoved;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// New timer view model from a timer
        /// </summary>
        /// <param name="timer"></param>
        public TimerViewModel(Timer timer)
        {
            Timer = timer;
            // Generate random light color
            BrushCreator();
            _originalColor = Brush.Color;
            _colorChanger = new DispatcherTimer();
            _colorChanger.Interval = TimeSpan.FromMilliseconds(100);
            _colorChanger.Tick += ColorChanger_Tick;
            // Th creation stages if the timer is new
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
                // Initialize loaded timer
                InitializeTimer();
            }
        }
        /// <summary>
        /// Initializes the seconds once the timer passes all the creation stages
        /// </summary>
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
        /// <summary>
        /// Do pretty colors for attention
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorChanger_Tick(object? sender, EventArgs e)
        {
            BrushCreator();
            OnPropertyChanged("Brush");
            OnPropertyChanged("GlowColor");
        }
        /// <summary>
        /// Generates a random light color
        /// </summary>
        public void BrushCreator()
        {
            Random rnd = new Random();
            int r = rnd.Next(160, 255);
            int g = rnd.Next(160, 255);
            int b = rnd.Next(160, 255);
            Brush = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            GlowColor = Brush.Color;
        }
        public void AddToDo(ToDo theToDoToAdd, bool loaded = false)
        {
            if (ToDos.Count > 0)
            {
                // Makes sure that the last todo was already created
                if (!ToDos[^1].Created)
                {
                    return;
                }
            }
            ToDoViewModel theViewModel = new ToDoViewModel(theToDoToAdd, Brush);
            theViewModel.ToDoRemoved += TheViewModel_ToDoRemoved;
            if (!loaded)
            {
                // Make sure that the underlying model gets the todo since we save the viewmodel
                Timer.ToDos.Add(theToDoToAdd);
            }
            ToDos.Add(theViewModel);
            ToDoAdded?.Invoke(this, new ToDoAdded { AddedToDo = theViewModel });
        }
        public void AddToDo(ToDoViewModel theToDoToAdd)
        {
            if (ToDos.Count > 0)
            {
                // Makes sure that the last todo was already created
                if (!ToDos[^1].Created)
                {
                    return;
                }
            }
            theToDoToAdd.ToDoRemoved += TheViewModel_ToDoRemoved;
            ToDos.Add(theToDoToAdd);
            ToDoAdded?.Invoke(this, new ToDoAdded { AddedToDo = theToDoToAdd });
        }
        private void TheViewModel_ToDoRemoved(object sender, RemoveToDo e)
        {
            RemoveToDo(e.ToDoToRemove);
        }

        public void RemoveToDo(ToDoViewModel theToDoToRemove)
        {
            ToDos.Remove(theToDoToRemove);
            Timer.ToDos.Remove(theToDoToRemove.ToDo);
            ToDoRemoved?.Invoke(this, new RemoveToDo { ToDoToRemove = theToDoToRemove });
        }
    }
}
