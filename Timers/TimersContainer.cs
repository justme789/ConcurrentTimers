using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Timers
{
    public class TimersContainer
    {
        DispatcherTimer timer;
        public ObservableCollection<Timer> Timers { get; set; } = new ObservableCollection<Timer>();
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
                Timer currentTimer = Timers[0];
                if (currentTimer.Finished)
                {
                    if (currentTimer.Cancelled)
                    {
                        Timers.RemoveAt(0);

                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                if (currentTimer.CurrVal <= 0)
                {
                    currentTimer.Finished = true;
                    return;
                }
                
                currentTimer.CurrVal--;
                currentTimer.DisplayVal = currentTimer.CurrVal + "";
            }
        }
    }

}
