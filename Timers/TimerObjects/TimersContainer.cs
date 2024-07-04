using System.Collections.Generic;

namespace Timers.TimerObjects
{
    public class TimersContainer
    {
        /// <summary>
        /// This class represent a row of timers
        /// </summary>
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
        public IEnumerable<Timer> GetTimers()
        {
            return Timers;
        }
    }
}
