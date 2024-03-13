using System;

namespace Timers.TimerObjects
{
    /// <summary>
    /// This class manages the event that is fired when a timer is removed
    /// </summary>
    public class RemoveTimerEvent : EventArgs
    {
        public TimerViewModel TimerToRemove { get; set; }
    }

}
