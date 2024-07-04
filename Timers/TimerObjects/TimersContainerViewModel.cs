using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Timers.ToDoList;

namespace Timers.TimerObjects
{
    /// <summary>
    /// The ViewModel of a timer container
    /// </summary>
    public class TimersContainerViewModel
    {
        /// <summary>
        /// The seconds dispatch timer that makes everything work
        /// </summary>
        private DispatcherTimer _ticker;
        /// <summary>
        /// Propogates an added todo all the way to the main window so it is added
        /// to the right side area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ToDoAddedHandler(object sender, ToDoAdded e);
        public event ToDoAddedHandler ToDoAdded;
        /// <summary>
        /// Propogates a removed todo all the way to the main window so it is removed
        /// from the right side area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ToDoRemover(object sender, RemoveToDo e);
        public event ToDoRemover ToDoRemoved;
        /// <summary>
        /// The actual timer object
        /// </summary>
        public TimersContainer Container { get; }
        /// <summary>
        /// The timers within this container
        /// </summary>
        public ObservableCollection<TimerViewModel> Timers { get; set; }
        /// <summary>
        /// Initializes a view model from a model
        /// create a new dispatch for the current container
        /// Start the timer
        /// </summary>
        /// <param name="theContainer"></param>
        public TimersContainerViewModel(TimersContainer theContainer)
        {
            Container = theContainer;
            Timers = new ObservableCollection<TimerViewModel>();
            _ticker = new DispatcherTimer();
            _ticker.Interval = TimeSpan.FromSeconds(1);
            _ticker.Tick += Ticker_Tick; ;
            _ticker.Start();
        }
        /// <summary>
        /// Add a timer and subscribe to the neccessary events
        /// Handles the loading logic of a saved timer as well
        /// </summary>
        /// <param name="timer">The timer to add</param>
        /// <param name="loaded">Wether it is a new or old timer</param>
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
                // Add the todos from the saved timer first
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
        /// <summary>
        /// Handles how a timer is treated at every tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ticker_Tick(object? sender, EventArgs e)
        {
            if (Timers.Count > 0)
            {
                // Always take the first timer
                TimerViewModel currentTimer = Timers[0];
                // Dont do anyhting if the timer is paused or still being created
                if (currentTimer.IsPaused || !currentTimer.Created)
                {
                    return;
                }
                // The timer reached 0
                if (currentTimer.Finished)
                {
                    // The timer was canceled i.e the X was pressed
                    if (currentTimer.Cancelled)
                    {
                        List<ToDoViewModel> theTodos = Timers[0].ToDos.ToList();
                        // Remove todos
                        foreach (ToDoViewModel toDo in theTodos)
                        {
                            Timers[0].RemoveToDo(toDo);
                            toDo.Remove();
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
                // Decrement the timer
                currentTimer.CurrentValue--;
                currentTimer.DisplayVal = currentTimer.CurrentValue + "";
            }
        }
    }
}
