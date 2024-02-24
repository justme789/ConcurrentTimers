using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Timers
{
    public class TimersContainer
    {
        DispatcherTimer timer;
        public ObservableCollection<TimerViewModel> Timers { get; set; } = new ObservableCollection<TimerViewModel>();
        public TimersContainer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object? sender, EventArgs e)
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
