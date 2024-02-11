using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Timers
{
    public class TimersContainer
    {
        DispatcherTimer timer;
        public ObservableCollection<TimerSituation> Timers { get; set; } = new ObservableCollection<TimerSituation>();
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
                TimerSituation currentTimer = Timers[0];
                if (currentTimer.CurrVal <= 0)
                {
                    Timers.RemoveAt(0);
                    return;
                }
                currentTimer.CurrVal--;
                currentTimer.DisplayVal = currentTimer.CurrVal + "";
            }
        }
    }

}
