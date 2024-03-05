using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Timers.TimerObjects
{
    public class TimersContainerViewModel
    {
        private DispatcherTimer _ticker;
        public delegate void ToDoAddedHandler(object sender, ToDoAdded e);

        // Declare the event based on the delegate
        public event ToDoAddedHandler ToDoAdded;
        public TimersContainer Container
        {
            get;
        }
        public ObservableCollection<TimerViewModel> Timers
        {
            get; set;
        }
        public TimersContainerViewModel(TimersContainer theContainer)
        {
            Container = theContainer;
            Timers = new ObservableCollection<TimerViewModel>();
            foreach (Timer timer in Container.Timers)
            {
                Timers.Add(new TimerViewModel(timer));
            }
            _ticker = new DispatcherTimer();
            _ticker.Interval = TimeSpan.FromSeconds(1);
            _ticker.Tick += Ticker_Tick; ;
            _ticker.Start();
        }
        public void AddTimer(Timer timer)
        {
            TimerViewModel timerViewModel = new TimerViewModel(timer);
            timerViewModel.RemoveTimer += TimerViewModel_RemoveTimer;
            timerViewModel.ToDoAdded += TimerViewModel_ToDoAdded;
            Timers.Add(timerViewModel);
            Container.Timers.Add(timer);
        }

        private void TimerViewModel_ToDoAdded(object sender, ToDoAdded e)
        {
            ToDoAdded?.Invoke(this, new ToDoAdded { AddedToDo = e.AddedToDo });
        }

        private void TimerViewModel_RemoveTimer(object sender, RemoveTimerEvent e)
        {
            RemoveTimer(e.TimerToRemove);
        }

        public void RemoveTimer(TimerViewModel timer)
        {
            Timers.Remove(timer);
            Container.Timers.Remove(timer.Timer);
        }
        private void Ticker_Tick(object? sender, EventArgs e)
        {
            if (Timers.Count > 0)
            {
                TimerViewModel currentTimer = Timers[0];
                if (currentTimer.IsPaused || !currentTimer.Created)
                {
                    return;
                }
                if (currentTimer.Finished)
                {
                    if (currentTimer.Cancelled)
                    {
                        Timers.RemoveAt(0);
                    }
                    return;
                }
                if (currentTimer.CurrentValue <= 0)
                {
                    currentTimer.Finished = true;
                    return;
                }
                currentTimer.CurrentValue--;
                currentTimer.DisplayVal = currentTimer.CurrentValue + "";
            }
        }
    }
}
