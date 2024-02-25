using System.Collections.Generic;

namespace Timers
{
    public class TimersContainer
    {
        public List<Timer> Timers { get; set; }
        public TimersContainer()
        {
            Timers = new List<Timer>();
        }
        public void AddTimer(Timer timer)
        {
            Timers.Add(timer);
        }
        public void RemoveTimer(Timer timer)
        {
            Timers.Add(timer);
        }
    }
}
