using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Timers.ToDoList;

namespace Timers.TimerObjects
{
    public class TimersContainerViewModel
    {
        private DispatcherTimer _ticker;

        public delegate void ToDoAddedHandler(object sender, ToDoAdded e);
        public event ToDoAddedHandler ToDoAdded;

        public delegate void ToDoRemover(object sender, RemoveToDo e);
        public event ToDoRemover ToDoRemoved;
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
            _ticker = new DispatcherTimer();
            _ticker.Interval = TimeSpan.FromSeconds(1);
            _ticker.Tick += Ticker_Tick; ;
            _ticker.Start();
        }
        public void AddTimer(Timer timer, bool loaded = false)
        {
            TimerViewModel timerViewModel = new TimerViewModel(timer);
            timerViewModel.RemoveTimer += TimerViewModel_RemoveTimer;
            timerViewModel.ToDoAdded += TimerViewModel_ToDoAdded;
            timerViewModel.ToDoRemoved += TimerViewModel_ToDoRemoved;
            if (!loaded)
            {
                Container.AddTimer(timer);
            }
            else
            {
                foreach (ToDo todo in timer.ToDos)
                {
                    timerViewModel.AddToDo(todo, true);
                }
            }
            Timers.Add(timerViewModel);
        }

        private void TimerViewModel_ToDoRemoved(object sender, RemoveToDo e)
        {
            ToDoRemoved?.Invoke(this, new RemoveToDo { ToDoToRemove = e.ToDoToRemove });

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
            Container.RemoveTimer(timer.Timer);
            List<ToDoViewModel> theTodos = timer.ToDos.ToList();
            foreach (ToDoViewModel toDo in theTodos)
            {
                timer.RemoveToDo(toDo);
            }
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
                        List<ToDoViewModel> theTodos = Timers[0].ToDos.ToList();
                        foreach (ToDoViewModel toDo in theTodos)
                        {
                            Timers[0].RemoveToDo(toDo);
                        }
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
